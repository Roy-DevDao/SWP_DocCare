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
//-------------------------------------------------------------------------------------------------------------
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

//-------------------------------------------------------------------------------------------------------------

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
//-------------------------------------------------------------------------------------------------------------


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
                return View(); // Trả về 404 nếu không tìm thấy

            }

            return View(appointment); // Trả về View với thông tin cuộc hẹn
        }

//-------------------------------------------------------------------------------------------------------------

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

//-------------------------------------------------------------------------------------------------------------
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

 //-------------------------------------------------------------------------------------------------------------
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

        //-------------------------------------------------------------------------------------------------------------

        public IActionResult AddHealthRecord(string appointmentDetail)
        {      
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var appointment = _appointmentDAO.GetAppointmentDetailById(appointmentDetail);
            if (appointment == null)
            {
                return NotFound();
            }

            var model = new HealthRecordViewModel
            {
                Pid = appointment.PatientId, // Lấy ID bệnh nhân từ cuộc hẹn
                Did = userId,                // ID bác sĩ là ID người dùng đăng nhập hiện tại
                OrderId = appointmentDetail,  // ID của cuộc hẹn
                DateExam = DateTime.Now       // Ngày hiện tại làm ngày khám bệnh
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddHealthRecord(HealthRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model is valid. Adding new health record with data: Diagnosis={0}, Description={1}, Note={2}, DateExam={3}",
                    model.Diagnosis, model.Description, model.Note, model.DateExam);

                var healthRecord = new HealthRecord
                {
                    RecordId = Guid.NewGuid().ToString(),
                    Pid = model.Pid,
                    Did = model.Did,
                    Oid = model.OrderId,
                    Diagnosis = model.Diagnosis,
                    Description = model.Description,
                    Note = model.Note,
                    DateExam = model.DateExam
                };

                _context.HealthRecords.Add(healthRecord);
                _context.SaveChanges();

                _logger.LogInformation("Health record added successfully with RecordId: {0}", healthRecord.RecordId);

                return RedirectToAction("ViewAppointment", new { id = model.Did });
            }

            _logger.LogWarning("Model is invalid. Errors: {0}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(model);
        }






        //-------------------------------------------------------------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
