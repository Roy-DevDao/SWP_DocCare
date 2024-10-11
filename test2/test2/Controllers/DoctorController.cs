using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test2.DAO;

namespace test2.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly AppointmentDAO _appointmentDAO;
        private readonly PatientDao _patientDao;
        private readonly FeedbackDAO _feedbackDao;

        // Did mặc định của bác sĩ
        private const string DefaultDoctorId = "a4";

        public DoctorController(ILogger<DoctorController> logger, AppointmentDAO appointmentDAO, PatientDao patientDao, FeedbackDAO feedbackDao)
        {
            _logger = logger;
            _appointmentDAO = appointmentDAO;
            _patientDao = patientDao;
            _feedbackDao = feedbackDao;
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

        public IActionResult ViewPatientDetail(string pid)
        {
            if (string.IsNullOrEmpty(pid))
            {
                return BadRequest("Patient ID is missing.");
            }

            var patient = _patientDao.GetPatientById(pid);

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            return View(patient); // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
