using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mscc.GenerativeAI;
using System.Diagnostics;
using System.Security.Claims;
using test2.DAO;
using test2.Data;
using test2.Models;
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

        public PatientController(ILogger<PatientController> logger, DocCareContext db, IVnPayService vnPayservice, UserDAO userDAO)
        {
            _logger = logger;
            dc = db;
            _vnPayservice = vnPayservice;
            _userDAO = userDAO;
        }

        // GET: Hiển thị trang hồ sơ với các ViewModel để cập nhật thông tin cá nhân và đổi mật khẩu
        public IActionResult Profile(string id)
        {
            _logger.LogInformation("OID received in Profile: {Oid}", id); // Log giá trị id

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            // Lấy thông tin bệnh nhân từ database
            var patient = (from p in dc.Patients
                           join a in dc.Accounts on p.Pid equals a.Id
                           where p.Pid == id
                           select new UpdateProfileViewModel
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
                _logger.LogWarning("No patient found with ID: {Oid}", id); // Log cảnh báo nếu không tìm thấy
                return RedirectToAction("Login", "Home");
            }

            // Tạo đối tượng PatientPageViewModel và gán các giá trị cần thiết
            var model = new PatientProfileViewModel
            {
                Patient = patient,
                ChangePassword = new ChangePasswordViewModel { PId = id }
            };

            // Truyền PatientPageViewModel vào view
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(PatientProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Trả về lại view với dữ liệu đã nhập để giữ nguyên thông tin nếu có lỗi
                return View("Profile", model);
            }

            // Xử lý cập nhật thông tin cá nhân
            var patient = await dc.Patients.FirstOrDefaultAsync(p => p.Pid == model.Patient.PId);
            if (patient != null)
            {
                patient.Name = model.Patient.Name;
                patient.Gender = model.Patient.Gender;
                patient.Dob = model.Patient.Dob;

                if (model.Patient.AvataUpload != null && model.Patient.AvataUpload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Patient.AvataUpload.CopyToAsync(memoryStream);
                        patient.PatientImg = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                await dc.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin bệnh nhân.";
            }

            return RedirectToAction("Profile", new { id = model.Patient.PId });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PatientProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Trả về lại view với dữ liệu đã nhập để giữ nguyên thông tin nếu có lỗi
                return View("Profile", model);
            }

            // Xử lý đổi mật khẩu
            var account = await dc.Accounts.FirstOrDefaultAsync(a => a.Id == model.ChangePassword.PId);
            if (account != null && account.Password == model.ChangePassword.OldPassword)
            {
                if (model.ChangePassword.NewPassword == model.ChangePassword.ConfirmNewPassword)
                {
                    account.Password = model.ChangePassword.NewPassword;
                    await dc.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Mật khẩu cũ không đúng hoặc không tìm thấy tài khoản.";
            }

            return RedirectToAction("Profile", new { id = model.ChangePassword.PId });
        }



        [HttpGet]
        [Route("Patient/AppointmentHistory")]
        public IActionResult AppointmentHistory()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            var user = _userDAO.GetLoggedInUser(User) ?? new UserProfileViewModel();
            ViewBag.User = user; // Truyền thông tin người dùng vào ViewBag
            var list = dc.Orders
                 .Include(o => o.Option)             // Include the Option navigation property
                 .ThenInclude(op => op.DidNavigation)
                 .ThenInclude(doctor => doctor.Specialty)
                 .ToList();
            return View(list);
        }

        [HttpGet]
        [Route("Patient/AppointmentHistory/{status?}")]
        public IActionResult AppointmentHistory(string status)
        {

            var orders = dc.Orders
                           .Include(o => o.Option)
                           .ThenInclude(op => op.DidNavigation)
                           .ThenInclude(doctor => doctor.Specialty)
                           .AsQueryable();  // Allows filtering on the query later

            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status == status);  // Filter by status
            }

            var orderList = orders.ToList();  // Fetch the filtered orders

            return View(orderList);  // Pass the filtered list to the view
        }

        public class OrderResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Specialty { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}

public JsonResult GetAppointmentByQuery(string query)
{
    var orders = dc.Orders
                   .Include(o => o.Option)
                   .ThenInclude(op => op.DidNavigation)
                   .ThenInclude(doctor => doctor.Specialty)
                   .AsQueryable();  // Allows filtering on the query later

    if (!string.IsNullOrEmpty(query))
    {
        orders = orders.Where(o => o.Option.DidNavigation.Name.Contains(query) || o.Option.DidNavigation.Specialty.SpecialtyName.Contains(query));  // Filter by status
    }
    

    var orderList = orders.ToList();
    if(orderList.Count > 0)
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
        return Json(data);
    }
    return Json("empty");

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

        public IActionResult BookingAppointment()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
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
