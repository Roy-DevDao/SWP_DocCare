using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using test2.DAO;
using test2.Data;
using test2.Models;
using test2.Models.DoctorModel;

namespace test2.Controllers
{
    //[Authorize(Roles = "2")]
    public class DoctorController : Controller
    {
        DocCareContext _context;
        private readonly ILogger<DoctorController> _logger;
        private readonly AppointmentDAO _appointmentDAO;
        private readonly FeedbackDAO _feedbackDao;
        private readonly UserDAO _userDAO;

        public DoctorController(ILogger<DoctorController> logger, AppointmentDAO appointmentDAO,  FeedbackDAO feedbackDao, DocCareContext ct, UserDAO _userDAO)
        {
            _logger = logger;
            _appointmentDAO = appointmentDAO;
            _feedbackDao = feedbackDao;
            _context = ct;
            _userDAO = _userDAO;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserId = User.FindFirst("Id")?.Value;
            }
            base.OnActionExecuting(context);
        }
        public IActionResult Feedback(string id, string? sortOrder = "asc")
        {
            // Lấy danh sách phản hồi của bác sĩ dựa trên Did
            var feedbacks = _feedbackDao.GetFeedbacksByDoctorId(id);
            return View(feedbacks);// This will render /Views/Staff/AppoitmentList.cshtml
        }

        public IActionResult Profile(string id)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            _logger.LogInformation("userId: {UserId}, id: {Id}", userId, id);


             _logger.LogInformation("OID received in DoctorProfile: {Oid}", id);

            if (userId != id)
            {
                _logger.LogWarning("User attempted to access a profile that does not belong to them: {UserId} tried to access {TargetId}", userId, id);
                return Forbid(); 
            }

            // Lấy thông tin bác sĩ từ cơ sở dữ liệu bằng id
            var doctor = (from d in _context.Doctors
                          join a in _context.Accounts on d.Did equals a.Id
                          join s in _context.Specialties on d.SpecialtyId equals s.SpecialtyId // Join với bảng chuyên khoa
                          where d.Did == id
                          select new BaseViewModel
                          {
                              DId = d.Did,                            
                              Name = d.Name,
                              DoctorImg = d.DoctorImg,
                              doctorProfile = new DoctorProfileViewModel
                              {
                                  Username = a.Username,
                                  Email = a.Email,
                                  Role = a.Role,
                                  Status = a.Status,
                                  Phone = d.Phone,
                                  Gender = d.Gender,
                                  Dob = d.Dob,
                                  Position = d.Position,
                                  Specialty = s.SpecialtyName, // Lấy tên chuyên khoa từ bảng chuyên khoa
                                  Description = d.Description,
                                  Price = d.Price,
                              }
                          }).ToList();

            // Kiểm tra xem bác sĩ có tồn tại không
            if (doctor == null)
            {
                _logger.LogWarning("No doctor found with ID: {id}", id); // Log cảnh báo nếu không tìm thấy
                return RedirectToAction("Login", "Home"); // Redirect về trang Login
            }

            // Trả về view cùng với model bác sĩ
            return View(doctor);
        }

        




        public IActionResult ViewAppointment(string id)
        {
            // Lấy các cuộc hẹn cho bác sĩ có ID được truyền vào
            var appointment = _appointmentDAO.GetDoctorAppointments(id);

            if (appointment == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy
            }

            return View(appointment); // Trả về View với thông tin cuộc hẹn
        }

        public IActionResult ViewAppointmentDetail(string appointmentDetail)
        {
            // Kiểm tra nếu không nhận được appointmentDetail
            if (string.IsNullOrEmpty(appointmentDetail))
            {
                return BadRequest("Appointment detail is missing.");
            }

            var appointment = _appointmentDAO.GetAppointmentDetailById(appointmentDetail);

            // Kiểm tra nếu không tìm thấy appointment
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Trả về view với model là appointment
            return View(appointment);
        }

        


        //public IActionResult HealthRecords(string id)
        //{
        //    // Retrieve the ID of the logged-in user
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    // Check if the user is authenticated
        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login", "Home"); // Redirect to login if not authenticated
        //    }

        //    // Check if the requested doctor ID matches the logged-in user ID
        //    if (string.IsNullOrEmpty(id) || userId != id)
        //    {
        //        _logger.LogWarning("Unauthorized access attempt: User {UserId} tried to access HealthRecord for doctor {TargetId}", userId, id);
        //        return Forbid(); // Return 403 Forbidden if IDs do not match
        //    }

        //    // Fetch health records for the doctor with the provided ID
        //    var healthRecords = _context.HealthRecords
        //                                .Where(hr => hr.Did == id)
        //                                .ToList();

        //    // Pass the health records data to the view
        //    return View(healthRecords);
        //}


        //public IActionResult ViewHealthRecords(string id)
        //{
        //    // Lấy các hồ sơ sức khỏe cho bác sĩ có ID được truyền vào
        //    var healthRecords = _patientDao.GetHealthRecordsByDoctorId(id);

        //    if (healthRecords == null || !healthRecords.Any())
        //    {
        //        return NotFound(); // Trả về 404 nếu không tìm thấy
        //    }

        //    return View(healthRecords); // Trả về View với thông tin hồ sơ sức khỏe
        //}


        //[HttpGet]
        //public IActionResult AddHealthRecord()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AddHealthRecord(HealthRecordViewModel model)
        //{
        //    var userId = User.FindFirst(ClaimTypes.Name)?.Value;

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    // Create and save the new health record
        //    var newRecord = new HealthRecord
        //    {
        //        Pid = model.Pid, // Patient ID (ensure the patient exists)
        //        Did = userId,    // Doctor ID of the logged-in doctor
        //        Diagnosis = model.Diagnosis,
        //        Description = model.Description,
        //        Note = model.Note,
        //        DateExam = model.DateExam
        //    };

        //    _context.HealthRecords.Add(newRecord);
        //    _context.SaveChanges();

        //    return RedirectToAction("HealthRecord"); // Redirect to the HealthRecord list
        //}





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
