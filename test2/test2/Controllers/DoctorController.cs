using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test2.DAO;
using test2.Data;

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
        private const string DefaultDoctorId = "a4";

        public DoctorController(ILogger<DoctorController> logger, AppointmentDAO appointmentDAO, PatientDao patientDao, FeedbackDAO feedbackDao, DocCareContext ct)
        {
            _logger = logger;
            _appointmentDAO = appointmentDAO;
            _patientDao = patientDao;
            _feedbackDao = feedbackDao;
            _context = ct;
        }

        public IActionResult Feedback(string did = DefaultDoctorId, string? sortOrder = "asc")
        {
            // Lấy danh sách phản hồi của bác sĩ dựa trên Did
            var feedbacks = _feedbackDao.GetFeedbacksByDoctorId(did);
            return View(feedbacks);// This will render /Views/Staff/AppoitmentList.cshtml
        }

        public IActionResult ViewAppointment()
        {
            // Get the appointments for the first doctor
            var appointments = _appointmentDAO.GetAppointmentsForFirstDoctor();

            // Pass the appointments to the view
            return View(appointments);
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

        public IActionResult ViewPatient(string did = DefaultDoctorId)
        {
            // Lấy danh sách bệnh nhân của bác sĩ dựa trên Did được truyền vào
            var patients = _patientDao.GetPatientsByDoctorId(did);

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
