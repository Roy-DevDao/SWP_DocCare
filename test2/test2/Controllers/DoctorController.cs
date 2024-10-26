using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test2.DAO;
using test2.Data;
using test2.Models;

namespace test2.Controllers
{
    [Authorize(Roles = "2")]
    public class DoctorController : Controller
    {
        DocCareContext _context;
        private readonly ILogger<DoctorController> _logger;
        private readonly AppointmentDAO _appointmentDAO;
        private readonly PatientDao _patientDao;
        private readonly FeedbackDAO _feedbackDao;
        private readonly UserDAO _userDAO;

        public DoctorController(ILogger<DoctorController> logger, AppointmentDAO appointmentDAO, PatientDao patientDao, FeedbackDAO feedbackDao, DocCareContext ct, UserDAO _userDAO)
        {
            _logger = logger;
            _appointmentDAO = appointmentDAO;
            _patientDao = patientDao;
            _feedbackDao = feedbackDao;
            _context = ct;
            _userDAO = _userDAO;
        }

        public IActionResult Feedback(string id, string? sortOrder = "asc")
        {
            // Lấy danh sách phản hồi của bác sĩ dựa trên Did
            var feedbacks = _feedbackDao.GetFeedbacksByDoctorId(id);
            return View(feedbacks);// This will render /Views/Staff/AppoitmentList.cshtml
        }

        public IActionResult Profile(string id)
        {
            _logger.LogInformation("OID received in DoctorProfile: {Oid}", id); // Log giá trị oid

            // Kiểm tra xem người dùng đã xác thực chưa
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            // Lấy thông tin bác sĩ từ cơ sở dữ liệu bằng id
            var doctor = (from d in _context.Doctors
                          join a in _context.Accounts on d.Did equals a.Id
                          join s in _context.Specialties on d.SpecialtyId equals s.SpecialtyId // Join với bảng chuyên khoa
                          where d.Did == id
                          select new DoctorProfileViewModel
                          {
                              DId = d.Did,
                              Username = a.Username,
                              Email = a.Email,
                              Role = a.Role,
                              Status = a.Status,
                              Name = d.Name,
                              Phone = d.Phone,
                              Gender = d.Gender,
                              Dob = d.Dob,
                              Position = d.Position,
                              Specialty = s.SpecialtyName, // Lấy tên chuyên khoa từ bảng chuyên khoa
                              Description = d.Description,
                              Price = d.Price,
                              DoctorImg = d.DoctorImg
                          }).FirstOrDefault();
            // Kiểm tra xem bác sĩ có tồn tại không
            if (doctor == null)
            {
                _logger.LogWarning("No doctor found with ID: {Oid}", id); // Log cảnh báo nếu không tìm thấy
                return RedirectToAction("Login", "Home"); // Redirect về trang Login
            }

            // Trả về view cùng với model bác sĩ
            return View(doctor);
        }


        public IActionResult ViewAppointment(string id)
        {
            // Lấy các cuộc hẹn cho bác sĩ có ID được truyền vào
            var appointment = _context.Orders
             .Include(o => o.PidNavigation) // Đưa thông tin bệnh nhân
             .FirstOrDefault(o => o.Oid == id); // Tìm kiếm cuộc hẹn theo Oid

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

        public IActionResult ViewPatient(string id)
        {
            // Lấy danh sách bệnh nhân của bác sĩ dựa trên Did được truyền vào
            var patients = _patientDao.GetPatientsByDoctorId(id);

            // Truyền danh sách bệnh nhân xuống view
            return View(patients); // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult ViewPatientDetail(string pid, string tab = "profile")
        {
            // Tìm bệnh nhân theo pid, bao gồm các đơn đặt hàng và tùy chọn liên quan
            var patient = _context.Patients
                .Include(p => p.Orders)
                .ThenInclude(o => o.Option)
                .FirstOrDefault(p => p.Pid == pid);

            // Nếu bệnh nhân không tồn tại, trả về lỗi 404
            if (patient == null)
            {
                return NotFound();
            }

            // Lấy danh sách cuộc hẹn từ các đơn đặt hàng của bệnh nhân
            var appointments = patient.Orders?.Select(o => new
            {
                // Kiểm tra xem Option và DateExam có null không
                //Date = o.Option?.DateExam?.ToString("yyyy-MM-dd") ?? "N/A",-----------
                //Time = o.Option?.DateExam?.ToString("HH:mm") ?? "N/A",------------------
                Status = o.Status ?? "N/A" // Kiểm tra xem Status có null không
            }).ToList();

            // Đảm bảo appointments không bị null, sử dụng danh sách trống nếu là null
            //ViewBag.Appointments = appointments ?? new List<object>(); // Chuyển đổi sang List<object> nếu appointments là null
            ViewBag.ActiveTab = tab;

            // Trả về view với mô hình bệnh nhân
            return View(patient);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
