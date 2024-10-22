using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using test2.Context;
using test2.Data;
using test2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace test2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DocCareContext dc;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, DocCareContext db, IConfiguration configuration)
        {
            _logger = logger;
            dc = db;
            _configuration = configuration;
        }
        //--------------------------------------------------------------------------------------------
        public IActionResult Index()
        {

            var doctors = dc.Doctors.Take(8).ToList();
            var specialties = dc.Specialties.Take(8).ToList();

            //truyen data thong qua view bag
            ViewBag.Doctors = doctors;
            ViewBag.Specialties = specialties;

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
                if (user.Password == password)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // Thêm role của user
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Điều hướng người dùng dựa trên role
                    switch (user.Role)
                    {
                        case 0: // Admin
                            return RedirectToAction("Index", "Admin");
                        case 1: // Staff
                            return RedirectToAction("AppointmentList", "Staff");
                        case 2: // Doctor
                            return RedirectToAction("ViewAppointment", "Doctor");
                        case 3: // Patient
                            return RedirectToAction("AppointmentHistory", "Patient");
                        default:
                            TempData["ErrorMessage"] = "Role not recognized.";
                            return RedirectToAction("Login");
                    }
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


        public IActionResult SignUp1(RegisterViewModel model)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                // Trả lại view với các lỗi xác thực
                return View("SignUp", model);
            }

            // Kiểm tra mật khẩu
            if (model.Password.Length < 8 || !Regex.IsMatch(model.Password, @"[A-Za-z]") || !Regex.IsMatch(model.Password, @"[0-9]"))
            {
                ModelState.AddModelError("", "Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ cái và số.");
                return View("SignUp", model);
            }

            // Kiểm tra xem tài khoản đã tồn tại chưa
            var existingAccount = dc.Accounts.FirstOrDefault(a => a.Username == model.Username || a.Email == model.Email);
            if (existingAccount != null)
            {
                ModelState.AddModelError("", "Tài khoản hoặc email đã tồn tại.");
                return View("SignUp", model);
            }

            try
            {
                // Tạo tài khoản mới
                var newAccount = new Account
                {
                    Id = Guid.NewGuid().ToString(), // Tạo ID ngẫu nhiên
                    Username = model.Username,
                    Password = model.Password,
                    //Password = BCrypt.Net.BCrypt.HashPassword(model.Password), // Mã hóa mật khẩu
                    Email = model.Email,
                    Role = 3, // Mặc định là Patient
                    Status = true // Có thể thay đổi tùy theo yêu cầu
                };

                // Thêm tài khoản vào cơ sở dữ liệu
                dc.Accounts.Add(newAccount);
                dc.SaveChanges();

                // Tạo bản ghi Patient
                var newPatient = new Patient
                {
                    Pid = newAccount.Id, // Sử dụng ID từ Account
                    Name = model.Name,
                    Phone = model.Phone,
                    Gender = model.Gender,
                    PatientImg = null // Hoặc đường dẫn đến ảnh người dùng
                };

                // Thêm patient vào cơ sở dữ liệu
                dc.Patients.Add(newPatient);
                dc.SaveChanges();

                // Ghi nhận thông báo thành công
                TempData["SuccessMessage"] = "Đăng ký thành công! Chuyển đến trang đăng nhập.";
                return RedirectToAction("Login", "Home"); // Điều hướng đến trang đăng nhập
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View("SignUp", model);
            }
        }


        //--------------------------------------------------------------------------------------------

        public IActionResult Profile()
            {
                return View();
            }
        //--------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult ForgotPass(string type)
        {
            if (type == "Password")
            {
                return View(new ResetPassModel());
            }
            else
            {
                return View(new ForgotPassModel());
            }
        }

        public async Task<IActionResult> ForgotPass(ForgotPassModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormType = "Email";
                return View(model);
            }

            var user = dc.Accounts.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Email không tồn tại trong hệ thống.";
                ViewBag.FormType = "Email";
                return View(model);
            }

            // Tạo mã xác nhận ngẫu nhiên
            var verificationCode = new Random().Next(100000, 999999).ToString();
            user.Password = verificationCode; // Lưu mã xác nhận tạm thời
            dc.SaveChanges();

            // Lấy thông tin cấu hình SMTP từ appsettings.json
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpHost = smtpSettings["Host"];
            var smtpPort = int.Parse(smtpSettings["Port"]);
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

            // Sử dụng thông tin cấu hình để gửi email
            using var smtpClient = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = "Password Reset Verification Code",
                Body = $"Your verification code is: {verificationCode}",
                IsBodyHtml = true
            };
            mailMessage.To.Add(user.Email);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                ViewBag.SuccessMessage = "Mã xác nhận đã được gửi về email của bạn.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi khi gửi email: {ex.Message}";
            }

            ViewBag.FormType = "Password";
            return View(new ResetPassModel { Email = model.Email });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FormType = "Password";
                return View(model);
            }

            var user = dc.Accounts.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Email không tồn tại trong hệ thống.";
                ViewBag.FormType = "Password";
                return View(model);
            }

            // Đặt lại mật khẩu
            user.Password = model.Password; // Lưu mật khẩu mới vào trường Password
            dc.SaveChanges();

            ViewBag.SuccessMessage = "Mật khẩu đã được thay đổi thành công. Bạn có thể đăng nhập lại.";
            return RedirectToAction("Login");
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
                DetailSpecialties = service.DetailSpecialties.ToList(),
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Home"); 
        }


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
