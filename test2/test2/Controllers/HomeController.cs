using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
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
//--------------------------------------------------------------------------------------------
        public IActionResult Index()
        {
            return View();
        }
        //--------------------------------------------------------------------------------------------

        public IActionResult DoctorList(string gender = "all", string speciality = "all", string sort = "all", int pageNumber = 1)
        {
            int pageSize = 9;

            var specialties = dc.Specialties.ToList();
            ViewBag.Specialties = specialties;

            var doctors = dc.Doctors.Include(d => d.Specialty).Include(d => d.Feedbacks).AsQueryable();

            // Lọc theo giới tính
            if (gender != "all")
            {
                bool isMale = gender == "true";
                doctors = doctors.Where(d => d.Gender == (isMale ? "Male" : "Female"));
            }

            // Lọc theo chuyên khoa
            if (speciality != "all")
            {
                doctors = doctors.Where(d => d.SpecialtyId == speciality);
            }

            // Sắp xếp bác sĩ
            switch (sort)
            {
                case "star":
                    doctors = doctors.OrderByDescending(d => d.Feedbacks.Any() ? d.Feedbacks.Average(f => f.Star ?? 0) : 0);
                    break;
                case "fee":
                    doctors = doctors.OrderBy(d => d.Price);
                    break;
                case "fee-":
                    doctors = doctors.OrderByDescending(d => d.Price);
                    break;
                default:
                    break;
            }

            // Phân trang và lấy danh sách bác sĩ
            var doctorList = doctors.Select(d => new DoctorViewModel
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

            // Tính tổng số bác sĩ
            var totalDoctors = doctors.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
            ViewBag.PageNumber = pageNumber;

            // Giữ lại các tham số lọc để truyền vào view
            ViewBag.Gender = gender;
            ViewBag.Speciality = speciality;
            ViewBag.Sort = sort;

            return View(doctorList);
        }

        //--------------------------------------------------------------------------------------------

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

        //--------------------------------------------------------------------------------------------

        public IActionResult Login()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Validate user credentials (replace this with your actual logic)

            var user = dc.Accounts.Where(dc => dc.Email == email).FirstOrDefault();
            if (user != null)
            {
                if(user.Password == password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, (user.Role == 1 ? "Patient" : "Doctor"))
                     };
                    var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var authProperties = new AuthenticationProperties
                    {
                        // Keep the user logged in for 30 minutes (adjust this as needed)
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }                
            }
            TempData["ErrorMessage"] = "Invalid login attempt.";
            return RedirectToAction("Login");
          
        }

        //--------------------------------------------------------------------------------------------

        public IActionResult SignUp()
            {
                return View();
            }
        //--------------------------------------------------------------------------------------------

        public IActionResult Profile()
            {
                return View();
            }
//--------------------------------------------------------------------------------------------

            public IActionResult ForgotPass()
            {
                return View();
            }
        //--------------------------------------------------------------------------------------------

        public IActionResult ServiceList(int pageNumber = 1, string search = "", string sort = "")
        {
            int pageSize = 6;

            // Lấy danh sách dịch vụ
            var services = dc.Specialties.AsQueryable();

            // Thực hiện tìm kiếm nếu có
            if (!string.IsNullOrWhiteSpace(search))
            {
                services = services.Where(s => s.SpecialtyName.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                switch (sort)
                {
                    case "1":
                        services = services.OrderBy(s => s.SpecialtyName); 
                        break;
                    case "2":
                        services = services.OrderByDescending(s => s.SpecialtyName); 
                        break;
                }
            }

            var totalServices = services.Count();

            var pagedServices = services
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalServices / (double)pageSize);
            ViewBag.PageNumber = pageNumber;
            ViewBag.Search = search; 
            ViewBag.Sort = sort; 

            return View(pagedServices);
        }



        //--------------------------------------------------------------------------------------------

        public IActionResult ServiceDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("ServiceList");
            }

            var service = dc.Specialties
                .Include(s => s.Doctors) 
                .FirstOrDefault(s => s.SpecialtyId == id);

            if (service == null)
            {
                return NotFound();
            }

            var feedbacks = dc.Feedbacks
            .AsEnumerable() 
        .Where(f => service.Doctors.Any(d => d.Did == f.Did))
        .ToList();


            double averageRating = feedbacks.Any() ? feedbacks.Average(f => f.Star ?? 0) : 0;
            int totalReviews = feedbacks.Count();

            var viewModel = new ServiceViewModel
            {
                SpecialtyId = service.SpecialtyId,
                SpecialtyName = service.SpecialtyName,
                SpecialtyImg = service.SpecialtyImg,
                ShortDescription = service.ShortDescription,
                LongDescription = service.LongDescription,
                Price = 500000, 
                Doctors = service.Doctors.ToList(), 
                Feedbacks = feedbacks,
                AverageRating = averageRating,
                TotalReviews = totalReviews
            };

            return View(viewModel);
        }

        //--------------------------------------------------------------------------------------------



        public IActionResult Contact()
            {
                return View();
            }
        //--------------------------------------------------------------------------------------------


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        public string GetSelected(string currentValue, string compareValue)
        {
            return currentValue == compareValue ? "selected" : "";
        }

    }
}
