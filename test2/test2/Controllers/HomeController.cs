using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test2.Data;
using test2.Models;

namespace test2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DocCareContext dc;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DocCareContext db)
        {
            _logger = logger;
            dc = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DoctorList(int pageNumber = 1)
        {
            int pageSize = 9;

            var specialties = dc.Specialties.ToList();
            ViewBag.Specialties = specialties;

            var doctors = dc.Doctors
                .Include(d => d.Specialty)
                .Include(d => d.Feedbacks)
                .Select(d => new DoctorViewModel
                {
                    DoctorId = d.Did,
                    Name = d.Name,
                    DoctorImg = d.DoctorImg,
                    Specialty = d.Specialty.SpecialtyName,
                    Price = d.Price ?? 0,
                    Position = d.Position,
                    NumberOfFeedbacks = d.Feedbacks.Count(),
                    Rating = d.Feedbacks.Any() ? d.Feedbacks.Average(f => f.Star ?? 0) : 0
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalDoctors = dc.Doctors.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
            ViewBag.PageNumber = pageNumber;

            return View(doctors);
        }

        public IActionResult DoctorDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DoctorList");
            }

            var doctor = dc.Doctors
                .Include(d => d.Specialty)
                .Include(d => d.Feedbacks)
                .FirstOrDefault(d => d.Did == id);

            if (doctor == null)
            {
                return NotFound(); 
            }

            var viewModel = new DoctorViewModel
            {
                DoctorId = doctor.Did,
                Name = doctor.Name,
                DoctorImg = doctor.DoctorImg,
                Specialty = doctor.Specialty?.SpecialtyName, 
                Price = doctor.Price ?? 0, 
                Position = doctor.Position,
                NumberOfFeedbacks = doctor.Feedbacks.Count(),
                Rating = doctor.Feedbacks.Any() ? doctor.Feedbacks.Average(f => f.Star ?? 0) : 0,
                Description = doctor.Description, 
                Feedbacks = doctor.Feedbacks.ToList()
            };

            return View(viewModel);
        }


        public IActionResult Login()
            {
                return View();
            }

            public IActionResult SignUp()
            {
                return View();
            }

            public IActionResult Profile()
            {
                return View();
            }

            public IActionResult ForgotPass()
            {
                return View();
            }

        public IActionResult ServiceList(int pageNumber = 1)
        {
            int pageSize = 6; // Số lượng dịch vụ trên mỗi trang

            // Lấy danh sách dịch vụ và tổng số dịch vụ
            var services = dc.Specialties.ToList();
            var totalServices = services.Count();

            // Lấy dịch vụ theo phân trang
            var pagedServices = services
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Thiết lập ViewBag cho tổng số trang và trang hiện tại
            ViewBag.TotalPages = (int)Math.Ceiling(totalServices / (double)pageSize);
            ViewBag.PageNumber = pageNumber;

            return View(pagedServices); // Trả về View với danh sách dịch vụ đã phân trang
        }


        public IActionResult ServiceDetail()
            {
                return View();
            }

            public IActionResult Contact()
            {
                return View();
            }


            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
