using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mscc.GenerativeAI;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using test2.DAO;
using test2.Data;
using test2.Models;
using test2.Models.Order;
using test2.Services;

namespace test2.Controllers
{
	//[Authorize(Roles = "3")]
	public class PatientController : Controller
	{
		private readonly ILogger<PatientController> _logger;
		private readonly DocCareContext dc;
		private readonly IVnPayService _vnPayservice;
		private readonly UserDAO _userDAO;
		private IMomoService _momoService;

		public PatientController(ILogger<PatientController> logger, DocCareContext db, IVnPayService vnPayservice, UserDAO userDAO, IMomoService momoService)
		{
			_logger = logger;
			dc = db;
			_vnPayservice = vnPayservice;
			_userDAO = userDAO;
			_momoService = momoService;
		}
		public IActionResult MomoPayment()
		{
			var user = _userDAO.GetLoggedInUser(User) ?? new UserProfileViewModel();
			ViewBag.User = user;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
		{
			var response = await _momoService.CreatePaymentAsync(model);
			return Redirect(response.PayUrl);
		}

		[HttpGet]
		public IActionResult MomoPaymentCallBack()
		{

			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);

			// Kiểm tra phản hồi từ MoMo
			if (response == null || string.IsNullOrEmpty(response.OrderId))
			{
				// Xử lý trường hợp không có thông tin đơn hàng
				TempData["ErrorMessage"] = "Không có thông tin đơn hàng. Thanh toán không thành công.";
				return RedirectToAction("MomoPayment");
			}

			// Kiểm tra mã trạng thái (giả sử "0" là thành công)
			if (response.ResponseCode != "0")
			{
				// Thanh toán thất bại
				TempData["ErrorMessage"] = $"Thanh toán thất bại: {response.ResponseCode}";
				return RedirectToAction("MomoPayment");
			}

			// Nếu thanh toán thành công, lưu đơn hàng vào database
			// Gọi hàm lưu đơn hàng ở đây

			// Chuyển hướng tới view hiển thị thanh toán thành công
			return View(response); // Truyền response vào view
		}


		public IActionResult Profile(string id)
		{
			_logger.LogInformation("OID received in Profile: {Oid}", id); // Log giá trị oid

			var isAuthenticated = User.Identity.IsAuthenticated;
			if (!isAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			// Fetch patient details from the database using the oid
			var patient = (from p in dc.Patients
						   join a in dc.Accounts on p.Pid equals a.Id
						   where p.Pid == id
						   select new PatientProfileViewModel
						   {
							   PId = p.Pid,
							   Username = a.Username,
							   Email = a.Email,
							   Role = a.Role,
							   Status = a.Status,
							   Name = p.Name,
							   Phone = p.Phone,
							   Gender = p.Gender,
							   Dob = p.Dob,
							   PatientImg = p.PatientImg
						   }).FirstOrDefault();

			if (patient == null)
			{
				_logger.LogWarning("No patient found with OID: {Oid}", id); // Log cảnh báo nếu không tìm thấy
				return RedirectToAction("Login", "Home");
			}

			// Pass the patient data to the view
			return View(patient);
		}



		public IActionResult AppointmentHistory()
		{
			var user = _userDAO.GetLoggedInUser(User) ?? new UserProfileViewModel();
			ViewBag.User = user;
			var isAuthenticated = User.Identity.IsAuthenticated;
			var id = User.Identity.Name;
			if (!isAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}
			ViewBag.Specialties = dc.Specialties.ToList();
			var list = dc.Orders
				 .Include(o => o.Option)             // Include the Option navigation property
				 .ThenInclude(op => op.DidNavigation)
				 .ThenInclude(doctor => doctor.Specialty).Where(o => o.Pid == id)
				 .OrderByDescending(o => o.DateOrder)
				 .ToList();
			int itemsPerPage = 5;

			var orderList = list.Take(itemsPerPage).ToList();
			return View(orderList);
		}




		public JsonResult GetAppointmentByQuery(string query, bool pending, bool complete, bool cancel, List<string> listFilter, string start, string end, int page)
		{

			var orders = dc.Orders
						   .Include(o => o.Option)
						   .ThenInclude(op => op.DidNavigation)
						   .ThenInclude(doctor => doctor.Specialty)
						   .Where(o => o.Pid == User.Identity.Name)
						   .AsQueryable();

			if (!string.IsNullOrEmpty(query))
			{
				orders = orders.Where(o => o.Option.DidNavigation.Name.Contains(query) || o.Option.DidNavigation.Specialty.SpecialtyName.Contains(query));  // Filter by status
			}

			if ((pending && cancel && complete) || (!pending && !complete && !cancel))
			{

			}
			else
			{
				if (!pending)
				{
					orders = orders.Where(o => o.Option.Status != "Pending");
				}
				if (!complete)
				{
					orders = orders.Where(o => o.Option.Status != "Complete");
				}
				if (!cancel)
				{
					orders = orders.Where(o => o.Option.Status != "Cancel");
				}
			}
			if (listFilter != null && listFilter.Count > 0)
			{
				orders = orders.Where(o => listFilter.Contains(o.Option.DidNavigation.SpecialtyId)); // Example filtering by doctor
			}
			if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
			{
				DateTime startDate = DateTime.Parse(start);
				DateTime endDate = DateTime.Parse(end);
				if (startDate < endDate)
				{
					orders = orders.Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate);
				}

			}


			int itemsPerPage = 5;
			if (page == 0)
			{
				page = 1;
			}
			var orderList = orders.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
			if (orderList.Count > 0)
			{

				List<OrderResponseDto> data = new List<OrderResponseDto>();
				for (int i = 0; i < orderList.Count; i++)
				{
					OrderResponseDto ord = new OrderResponseDto
					{
						Id = orderList[i].Oid,
						Name = orderList[i].Option.DidNavigation.Name,
						Image = orderList[i].Option.DidNavigation.DoctorImg,
						Specialty = orderList[i].Option.DidNavigation.Specialty.SpecialtyName,
						Date = (DateTime)orderList[i].DateOrder,
						Status = orderList[i].Option.Status
					};



					data.Add(ord);

				}
				var total = orders.Count();
				if (page == 1)
				{
					return Json(new { data = data, total = total });
				}
				return Json(new { data = data });
			}
			return Json(new { total = -1 });

		}

		public IActionResult AppointmentDetail(string oid)
		{
			var user = _userDAO.GetLoggedInUser(User) ?? new UserProfileViewModel();
			ViewBag.User = user;
			var order = dc.Orders
						  .Include(o => o.Option)
						  .ThenInclude(op => op.DidNavigation)
						  .ThenInclude(doctor => doctor.Specialty)
						  .Include(o => o.PidNavigation)
						  .Include(o => o.HealthRecords)
						  .ThenInclude(hr => hr.DidNavigation)
						  .FirstOrDefault(o => o.Oid == oid);

			if (order == null)
			{
				return NotFound(); // Handle the case when the order is not found
			}

			return View(order);  // Pass the order object to the view
		}

		[HttpGet]
		public IActionResult BookingAppointment(string doctorid)
		{
			var user = _userDAO.GetLoggedInUser(User) ?? new UserProfileViewModel();
			ViewBag.User = user;
			if (!string.IsNullOrWhiteSpace(doctorid))
			{
				// Fetch the doctor including their specialty
				Doctor doctor = dc.Doctors.Include(d => d.Specialty).FirstOrDefault(d => d.Did == doctorid);

				if (doctor != null)
				{
					// Define the date range: today and the next 7 days
					DateTime today = DateTime.Now.Date;  // Current date (no time part)
					DateTime next7Days = today.AddDays(7);  // Next 7 days (including today)

					// Get the schedule for the doctor in the next 7 days
					var schedule = dc.Options
						.Where(o => o.Did == doctor.Did && o.DateWork >= today && o.DateWork <= next7Days)
						.ToList();


					var viewModel = new DoctorScheduleViewModel
					{
						Doctor = doctor,
						Schedule = schedule,
						Today = today
					};
					// You can pass 'schedule' to the view if needed
					return View(viewModel);
				}
			}


			return View();
		}

		[HttpPost]
		public async Task<JsonResult> ProcessBooking(string doctorid, string desc, string time)
		{

			if (!string.IsNullOrEmpty(doctorid) && !string.IsNullOrEmpty(time) && !string.IsNullOrEmpty(desc))
			{
				if (dc.Options.Any(o => o.Did == doctorid && o.DateWork == DateTime.Parse(time) && o.Status != "Canceled"))
				{
					return Json(new { err = "Slot was Book by other people" });
				}
				else
				{
					Random random = new Random();
					int buff = random.Next(1000000, 9999999);
					string optid = "opt" + buff;
					string ordid = "ord" + buff;
					using (var transaction = dc.Database.BeginTransaction())
					{
						try
						{
							// Tạo đối tượng Option
							Option op = new Option
							{
								OptionId = optid,
								Status = "Pending",
								Did = doctorid,
								DateWork = DateTime.Parse(time),
							};

							// Thêm vào bảng Option và lưu
							dc.Options.Add(op);
							await dc.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

							// Tạo đối tượng Order
							Order order = new Order
							{
								Oid = ordid,
								Pid = User.Identity.Name,
								OptionId = op.OptionId,
								DateOrder = DateTime.Now,
								Status = "Pending",
								Symptom = desc,
							};

							// Thêm vào bảng Order và lưu
							dc.Orders.Add(order);
							await dc.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

							// Commit transaction
							transaction.Commit();

							return Json(new { success = true });
						}
						catch (Exception ex)
						{
							// Rollback transaction
							transaction.Rollback();
							Debug.WriteLine($"Error saving to database: {ex.Message}");
							return Json(new { error = "Error saving to database" });
						}
					}


				}
			}
			else return Json(new { error = "Data is invalid" });
		}

		public async Task<JsonResult> CancelAppointment(string oid)
		{
			using (var transaction = dc.Database.BeginTransaction())
			{
				try
				{
					Order order = dc.Orders.Include(o => o.Option).FirstOrDefault(o => o.Oid == oid);
					if (order == null)
					{
						return Json(new { error = "Order is't found" });
					}
					if (order.Status != "Pending" && order.Status != "Confirm")
					{
						return Json(new { error = "Order can not be canceled" });

					}
					DateTime timenow = DateTime.Now.AddHours(5);
					if (order.Option.DateWork < timenow)
					{
						return Json(new { error = "Your order exceed allowed time for canceling" });
					}
					order.Option.Status = "Canceled";
					dc.Entry(order.Option).State = EntityState.Modified;
					await dc.SaveChangesAsync();

					// Update the status of the Order entity
					order.Status = "Canceled";
					dc.Entry(order).State = EntityState.Modified;

					// Save the changes to both entities
					await dc.SaveChangesAsync();

					
					 
					transaction.Commit();
					return Json(new { success = true });


				}
				catch (Exception ex)
				{
					transaction.Rollback();
					return Json(new { error = "Cancel failed. Please call to hospital for detail" });
				}
			}
			return Json(new { error = "fsdf" });
		}

		[HttpPost]
		public async Task<JsonResult> SendMessage(string message, string doctorid, int star)
		{
			Random random = new Random();
			if (User.Identity.Name != null)
			{
				Patient patient = dc.Patients.FirstOrDefault(p => p.Pid == User.Identity.Name);
				if (patient != null)
				{
					for (int i = 0; i < 10; i++)
					{
						int buff = random.Next(1000000, 9999999);
						string fbid = "f" + buff;
						if (!dc.Feedbacks.Any(f => f.FeedbackId == fbid))
						{
							Feedback feedback = new Feedback
							{
								FeedbackId = fbid,
								Did = doctorid,
								Star = star,
								Pid = User.Identity.Name,
								Description = message,
								DateCmt = DateTime.Now,
								Name = patient.Name,

							};
							try
							{
								dc.Feedbacks.Add(feedback);
								await dc.SaveChangesAsync();
								return Json(new { success = true, name = feedback.Name, description = feedback.Description, star = feedback.Star, ngay = feedback.DateCmt.ToString(), image = patient.PatientImg });
							}
							catch (Exception ex)
							{
								return Json(new { error = "save feedback fail" });
							}


						}
					}

				}


			}



			return Json(new { error = "user invalid" });
		}


		public IActionResult BookingService()
		{
			var user = _userDAO.GetLoggedInUser(User) ?? new UserProfileViewModel();
			ViewBag.User = user;
			var isAuthenticated = User.Identity.IsAuthenticated;
			if (!isAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}
			return View();  // This will render /Views/Staff/ServiceAppointList.cshtml
		}


		public IActionResult VnPayment()
		{
			var isAuthenticated = User.Identity.IsAuthenticated;
			if (!isAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}
			var vnPayModel = new VnPaymentRequestModel
			{
				Amount = 100000,
				CreatedDate = DateTime.Now,
				Description = "PHAN THANH BAO 0987367341",
				FullName = "PHAN THANH BAO",
				OrderId = new Random().Next(1000, 100000)
			};
			return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
		}


		public IActionResult ServiceHistory()
		{
			var isAuthenticated = User.Identity.IsAuthenticated;
			if (!isAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}
			return View();
		}

		//[Authorize]
		//[HttpPost]
		//public IActionResult Checkout(CheckoutVM model, string payment = "COD")
		//{
		//    if (ModelState.IsValid)
		//    {
		//        if (payment == "Thanh toán VNPay")
		//        {
		//            var vnPayModel = new VnPaymentRequestModel
		//            {
		//                Amount = Cart.Sum(p => p.ThanhTien),
		//                CreatedDate = DateTime.Now,
		//                Description = $"{model.HoTen} {model.DienThoai}",
		//                FullName = model.HoTen,
		//                OrderId = new Random().Next(1000, 100000)
		//            };
		//            return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
		//        }

		//        var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
		//        var khachHang = new KhachHang();
		//        if (model.GiongKhachHang)
		//        {
		//            khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
		//        }

		//        var hoadon = new HoaDon
		//        {
		//            MaKh = customerId,
		//            HoTen = model.HoTen ?? khachHang.HoTen,
		//            DiaChi = model.DiaChi ?? khachHang.DiaChi,
		//            DienThoai = model.DienThoai ?? khachHang.DienThoai,
		//            NgayDat = DateTime.Now,
		//            CachThanhToan = "COD",
		//            CachVanChuyen = "GRAB",
		//            MaTrangThai = 0,
		//            GhiChu = model.GhiChu
		//        };

		//        db.Database.BeginTransaction();
		//        try
		//        {

		//            db.Add(hoadon);
		//            db.SaveChanges();

		//            var cthds = new List<ChiTietHd>();
		//            foreach (var item in Cart)
		//            {
		//                cthds.Add(new ChiTietHd
		//                {
		//                    MaHd = hoadon.MaHd,
		//                    SoLuong = item.SoLuong,
		//                    DonGia = item.DonGia,
		//                    MaHh = item.MaHh,
		//                    GiamGia = 0
		//                });
		//            }
		//            db.AddRange(cthds);
		//            db.SaveChanges();
		//            db.Database.CommitTransaction();

		//            HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

		//            return View("Success");
		//        }
		//        catch
		//        {
		//            db.Database.RollbackTransaction();
		//        }
		//    }

		//    return View(Cart);
		//}

		[Authorize]
		//public IActionResult PaymentFail()
		//{
		//    // Ghi nhận thông báo lỗi
		//    TempData["Message"] = "Thanh toán thất bại. Vui lòng thử lại.";
		//    // Chuyển hướng về trang chủ
		//    return RedirectToAction("Index", "Home"); // Giả sử 'Index' là phương thức trong 'HomeController'
		//}

		//[Authorize]
		//public IActionResult PaymentSuccess()
		//{
		//    // Chuyển hướng về trang lịch sử đặt chỗ (appointment history)
		//    return RedirectToAction("AppointmentHistory", "Appointment"); // Giả sử 'AppointmentHistory' là phương thức trong 'AppointmentController'
		//}

		[Authorize]
		public IActionResult VnPaymentCallBack()
		{
			var response = _vnPayservice.PaymentExecute(Request.Query);

			if (response == null || response.VnPayResponseCode != "00")
			{
				TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}. Bạn sẽ quay lại trang chủ.";
				return RedirectToAction("Index", "Home");
			}



			TempData["Message"] = $"Thanh toán VNPay thành công! Bạn sẽ được chuyển đến lịch hẹn.";
			return RedirectToAction("AppointmentHistory", "Home"); // Chuyển hướng đến trang lịch hẹn
		}



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
