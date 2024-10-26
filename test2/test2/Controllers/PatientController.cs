using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using test2.Data;
using test2.Models;
using test2.Services;
using static NuGet.Packaging.PackagingConstants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace test2.Controllers
{
    [Authorize(Roles = "3")]
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly DocCareContext dc;
        private readonly IVnPayService _vnPayservice;

        public PatientController(ILogger<PatientController> logger, DocCareContext db, IVnPayService vnPayservice)
        {
            _logger = logger;
            dc = db;
            _vnPayservice = vnPayservice;
        }
        [HttpGet]
        [Route("Patient/AppointmentHistory")]
        public IActionResult AppointmentHistory()
        {
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
            if(page == 0)
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
				return Json(new { data = data});
            }
            return Json(new { total = -1 });

        }

        public IActionResult AppointmentDetail(string oid)
        {
            // Fetch the order details including all necessary related information
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
                if (dc.Options.Any(o => o.Did == doctorid && o.DateWork == DateTime.Parse(time)))
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



        public IActionResult BookingService()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();  // This will render /Views/Staff/ServiceAppointList.cshtml
        }

        public IActionResult Payment()
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
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
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
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }

        [Authorize]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }


            // Lưu đơn hàng vô database

            TempData["Message"] = $"Thanh toán VNPay thành công";
            return RedirectToAction("PaymentSuccess");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
