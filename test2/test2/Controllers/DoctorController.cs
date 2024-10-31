using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using test2.DAO;
using test2.Data;
using test2.Models.DoctorModel;

namespace test2.Controllers
{
    //[Authorize(Roles = "2")]
    public class DoctorController : Controller
    {
        DocCareContext _context;
        private readonly ILogger<DoctorController> _logger;
        private readonly AppointmentDAO _appointmentDAO;
        private readonly PatientDao _patientDao;
        private readonly FeedbackDAO _feedbackDao;
        private readonly UserDAO _userDAO;

        public DoctorController(ILogger<DoctorController> logger, AppointmentDAO appointmentDAO, PatientDao patientDao, FeedbackDAO feedbackDao, DocCareContext ct, UserDAO ud)
        {
            _logger = logger;
            _appointmentDAO = appointmentDAO;
            _patientDao = patientDao;
            _feedbackDao = feedbackDao;
            _context = ct;
            _userDAO = ud;
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

            // Chuyển đổi phản hồi thành danh sách BaseViewModel
            var feedbackViewModels = feedbacks.Select(f => new BaseViewModel
            {
                feedbackView = new FeedbackViewModel
                {
                    FeedbackId = f.FeedbackId,
                    PatientName = f.PidNavigation?.Name,
                    DateCmt = f.DateCmt,
                    Star = f.Star,
                    Description = f.Description
                }
            }).ToList();

            // Sắp xếp theo thứ tự tăng hoặc giảm sao
            feedbackViewModels = sortOrder == "desc"
                ? feedbackViewModels.OrderByDescending(f => f.feedbackView.Star).ToList()
                : feedbackViewModels.OrderBy(f => f.feedbackView.Star).ToList();

            // Truyền danh sách BaseViewModel vào View
            return View(feedbackViewModels);
        }



        public IActionResult Profile(string id)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            }

            // Log giá trị oid
            _logger.LogInformation("OID received in DoctorProfile: {Oid}", id);

            // Kiểm tra xem ID của người dùng có khớp với ID trong URL không
            if (userId != id)
            {
                _logger.LogWarning("User attempted to access a profile that does not belong to them: {UserId} tried to access {TargetId}", userId, id);
                return Forbid(); // Ngăn chặn truy cập nếu ID không khớp
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
                _logger.LogWarning("No doctor found with ID: {Oid}", id); // Log cảnh báo nếu không tìm thấy
                return RedirectToAction("Login", "Home"); // Redirect về trang Login
            }

            // Trả về view cùng với model bác sĩ
            return View(doctor);
        }


        public IActionResult ViewAppointment(string id)
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            }

            // Log giá trị oid
            _logger.LogInformation("OID received in DoctorProfile: {Oid}", id);

            // Kiểm tra xem ID của người dùng có khớp với ID trong URL không
            if (userId != id)
            {
                _logger.LogWarning("User attempted to access a profile that does not belong to them: {UserId} tried to access {TargetId}", userId, id);
                return Forbid(); // Ngăn chặn truy cập nếu ID không khớp
            }

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
            if (string.IsNullOrEmpty(appointmentDetail))
            {
                return BadRequest("Appointment detail is missing.");
            }

            var appointmentDetailViewModel = _appointmentDAO.GetAppointmentDetailById(appointmentDetail);

            if (appointmentDetailViewModel == null)
            {
                return NotFound("Appointment not found.");
            }

            // Tạo danh sách BaseViewModel và gán appointmentDetail vào
            var baseViewModel = new BaseViewModel
            {
                appointmentDetail = appointmentDetailViewModel
            };

            // Truyền danh sách vào View
            return View("ViewAppointmentDetail", new List<BaseViewModel> { baseViewModel });
        }

        



        public IActionResult ViewPatient(string id)
        {
            // Lấy danh sách bệnh nhân dựa trên bác sĩ có Did = id
            var patients = _patientDao.GetPatientsByDoctorId(id);

            // Gói dữ liệu bệnh nhân vào BaseViewModel
            var baseViewModelList = patients.Select(p => new BaseViewModel
            {
                patientView = p  // Gán từng PatientViewModel vào BaseViewModel
            }).ToList();

            // Gán ID của bác sĩ vào ViewData để hiển thị trên giao diện
            ViewData["DoctorId"] = id;

            // Truyền danh sách BaseViewModel vào View
            return View(baseViewModelList);
        }



        public IActionResult ViewPatientDetail(string pid, string tab = "profile")
        {
            // Tìm bệnh nhân theo pid, bao gồm các đơn hàng và tùy chọn liên quan
            var patient = _context.Patients
                .Include(p => p.Orders)
                .ThenInclude(o => o.Option)
                .FirstOrDefault(p => p.Pid == pid);

            // Nếu bệnh nhân không tồn tại, trả về lỗi 404
            if (patient == null)
            {
                return NotFound();
            }

            // Tạo model chi tiết bệnh nhân
            var patientDetail = new PatientDetailViewModel
            {
                Pid = patient.Pid,
                Name = patient.Name,
                Phone = patient.Phone,
                Email = patient.PidNavigation?.Email,
                Dob = patient.Dob,
                Appointments = patient.Orders.Select(o => new PatientDetailViewModel.AppointmentViewModel
                {
                    Date = o.DateOrder?.ToString("yyyy-MM-dd"),
                    Time = o.DateOrder?.ToString("HH:mm"),
                    Status = o.Status ?? "N/A"
                }).ToList()
            };

            ViewBag.ActiveTab = tab; // Chuyển tab (profile/appointment)

            // Gói dữ liệu chi tiết bệnh nhân vào BaseViewModel
            var baseViewModel = new BaseViewModel
            {
                patientDetail = patientDetail
            };

            // Truyền danh sách BaseViewModel vào View
            return View("ViewPatientDetail", new List<BaseViewModel> { baseViewModel });
        }


        [HttpGet]
        public IActionResult AddHealthRecord(string appointmentId)
        {
            if (string.IsNullOrEmpty(appointmentId))
            {
                return BadRequest("Thiếu thông tin mã cuộc hẹn.");
            }

            var appointment = _context.Orders
                .Include(o => o.PidNavigation)
                .Include(o => o.Option)
                    .ThenInclude(op => op.DidNavigation)
                .FirstOrDefault(o => o.Oid == appointmentId);

            _logger.LogInformation(appointment.PidNavigation.Name);

            if (appointment == null)
            {
                return NotFound("Không tìm thấy thông tin cuộc hẹn.");
            }

            var baseViewModel = new BaseViewModel
            {
                DId = appointment.Option.Did,
                Name = appointment.Option.DidNavigation.Name,
                DoctorImg = appointment.Option.DidNavigation.DoctorImg,
                appointmentlist = new AppointmentViewModel()
                {
                    AppointmentId = appointmentId,
                },
                healthRecord = new HealthRecordViewModel
                {
                    PatientName = appointment.PidNavigation.Name,
                    AppointmentId = appointmentId,
                    DateExam = DateTime.Now // Ngày khám mặc định là hôm nay
                }
            };
            _logger.LogInformation("Thông tin Model trước khi trả về view AddHealthRecord: " +
                          "DoctorId = {DId}, " +
                          "PatientName = {PatientName}, " +
                          "AppointmentId = {AppointmentId}, " +
                          "DateExam = {DateExam}",
                          baseViewModel.DId,
                          baseViewModel.Name,
                          baseViewModel.appointmentlist.AppointmentId,
                          baseViewModel.healthRecord.DateExam);

            return View("AddHealthRecord", new List<BaseViewModel> { baseViewModel });
        }




        [HttpPost]
        public IActionResult AddHealthRecord(string appointmentId, string diagnosis, string description, string note, DateTime dateExam)
        {
            // Kiểm tra nếu `appointmentId` rỗng hoặc `ModelState` không hợp lệ
            if (string.IsNullOrEmpty(appointmentId) || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "Thông tin cuộc hẹn hoặc thông tin nhập không hợp lệ.");
                return View("AddHealthRecord"); // Trả về lại view nếu có lỗi
            }

            // Lấy thông tin cuộc hẹn từ database
            var appointment = _context.Orders
                .Include(o => o.PidNavigation) // Thông tin bệnh nhân
                .Include(o => o.Option)// Thông tin Option để truy cập bác sĩ
                    .ThenInclude(op => op.DidNavigation)
                .FirstOrDefault(o => o.Oid == appointmentId);

            // Kiểm tra nếu không tìm thấy cuộc hẹn
            if (appointment == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin cuộc hẹn.");
                return View("AddHealthRecord"); // Trả về lại view nếu không tìm thấy cuộc hẹn
            }

            // Log thông tin của các thuộc tính trước khi kiểm tra null
            _logger.LogInformation("Thông tin cuộc hẹn trước khi kiểm tra null:");
            _logger.LogInformation("Appointment ID: {AppointmentId}", appointment?.Oid);
            _logger.LogInformation("Patient ID (PId): {PatientId}", appointment?.PidNavigation?.Pid);
            _logger.LogInformation("Patient Name: {PatientName}", appointment?.PidNavigation?.Name);
            _logger.LogInformation("Option ID: {OptionId}", appointment?.Oid);
            _logger.LogInformation("Doctor ID (DId): {DoctorId}", appointment?.Option.Did);
            _logger.LogInformation("Doctor Name: {DoctorName}", appointment?.Option?.DidNavigation?.Name);


            // Tạo bản ghi HealthRecord mới
            var healthRecord = new HealthRecord
            {
                RecordId = Guid.NewGuid().ToString(), // Tạo ID duy nhất cho HealthRecord
                Pid = appointment.Pid, // ID bệnh nhân từ thông tin cuộc hẹn
                Did = appointment.Option.Did, // ID bác sĩ từ Option
                Oid = appointment.Oid,
                Diagnosis = diagnosis,
                Description = description,
                Note = note,
                DateExam = dateExam
            };

            // Thêm bản ghi vào database
            _context.HealthRecords.Add(healthRecord);
            _context.SaveChanges();

            appointment.Option.Status = "Complete";
            // Lưu tất cả thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Ghi log thông tin bản ghi HealthRecord vừa được tạo
            _logger.LogInformation("Đã thêm hồ sơ sức khỏe mới cho cuộc hẹn với ID: {AppointmentId}, " +
                                   "Bệnh Nhân: {PatientName}, Bác Sĩ: {DoctorId}, Ngày Khám: {DateExam}",
                                   appointmentId,
                                   appointment.PidNavigation.Name,
                                   appointment.Option.Did,
                                   dateExam);

            // Chuyển hướng về trang chi tiết cuộc hẹn sau khi thêm thành công
            return RedirectToAction("ViewAppointment", new { id = appointment.Option.Did });
        }   



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}