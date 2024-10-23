using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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
            if (!isAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
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