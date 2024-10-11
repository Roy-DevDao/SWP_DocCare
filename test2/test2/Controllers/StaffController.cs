using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test2.Context;
using test2.Data;
using test2.Models;

namespace test2.Controllers
{
    public class StaffController : Controller
    {
        private readonly DocCareContext dc;

        private readonly ILogger<StaffController> _logger;

        public StaffController(ILogger<StaffController> logger, DocCareContext dc)
        {
            _logger = logger;
            this.dc = dc;

        }
        //-------------------------------------------------------------------------------------------------------------
        public IActionResult AppoitmentList(string search_doctor = "", string status = "all", int pageNumber = 1)
        {
            int pageSize = 10; // Số lượng cuộc hẹn trên mỗi trang

            // Lấy danh sách hẹn
            var appointmentsQuery = dc.Orders.Include(o => o.PidNavigation)
                                              .Include(o => o.Option)
                                              .ThenInclude(opt => opt.DidNavigation)
                                              .AsQueryable();

            // Thực hiện tìm kiếm theo tên bác sĩ
            if (!string.IsNullOrEmpty(search_doctor))
            {
                appointmentsQuery = appointmentsQuery.Where(o => o.Option.DidNavigation.Name.Contains(search_doctor));
            }

            // Thực hiện lọc theo trạng thái
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                appointmentsQuery = appointmentsQuery.Where(o => o.Status == status);
            }

            // Chọn các trường cần thiết và chuyển đổi sang AppointmentViewModel
            var appointments = appointmentsQuery.Select(o => new AppointmentViewModel
            {
                AppointmentId = o.Oid,
                PatientName = o.PidNavigation.Name,
                DoctorName = o.Option.DidNavigation.Name,
                AppointmentDate = o.Option.DateExam.HasValue ? o.Option.DateExam.Value : DateTime.MinValue,
                Status = o.Status
            })
            .Skip((pageNumber - 1) * pageSize) // Bỏ qua số lượng cuộc hẹn đã hiển thị ở các trang trước
            .Take(pageSize) // Lấy số lượng cuộc hẹn tương ứng với trang hiện tại
            .ToList();

            // Tính tổng số cuộc hẹn
            var totalAppointments = appointmentsQuery.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalAppointments / (double)pageSize); // Tổng số trang
            ViewBag.PageNumber = pageNumber; // Trang hiện tại

            // Giữ lại các tham số lọc để truyền vào view
            ViewBag.SearchDoctor = search_doctor;
            ViewBag.Status = status;

            return View(appointments);
        }



        //-------------------------------------------------------------------------------------------------------------

        public IActionResult AppointmentDetail(string id)
        {
            // Retrieve the appointment details based on the appointment ID (OId)
            var appointment = dc.Orders
                .Include(o => o.PidNavigation) // Patient details
                .Include(o => o.Option)
                    .ThenInclude(opt => opt.DidNavigation) // Doctor details
                .ThenInclude(d => d.Specialty) // Include doctor's specialization
                .Where(o => o.Oid == id)
                .Select(o => new AppointmentDetailViewModel
                {
                    AppointmentId = o.Oid,
                    PatientName = o.PidNavigation.Name,
                    PatientPhone = o.PidNavigation.Phone,
                    PatientGender = o.PidNavigation.Gender,
                    PatientDOB = o.PidNavigation.Dob,
                    PatientImage = o.PidNavigation.PatientImg,
                    DoctorName = o.Option.DidNavigation.Name,
                    DoctorPhone = o.Option.DidNavigation.Phone,
                    DoctorSpecialization = o.Option.DidNavigation.Specialty.SpecialtyName,
                    DoctorImage = o.Option.DidNavigation.DoctorImg,
                    DoctorGender = o.Option.DidNavigation.Gender,
                    AppointmentDate = o.Option.DateExam.HasValue ? o.Option.DateExam.Value : DateTime.MinValue,
                    AppointmentTime = o.Option.DateExam.HasValue ? o.Option.DateExam.Value.ToString("hh:mm tt") : "N/A",
                    Status = o.Status,
                    Fee = o.Option.DidNavigation.Price ?? 0,
                    SupportingStaff = "Nguyễn Văn C",  // Static for now
                    ConsultationInfo = o.Symptom,  // Store examination details
                })
                .FirstOrDefault();

            // If appointment is not found, return NotFound or a similar view
            if (appointment == null)
            {
                return NotFound();
            }

            // Pass the data to the view
            return View(appointment);
        }


        //-------------------------------------------------------------------------------------------------------------

        public IActionResult ServiceAppointList(string search_service = "", string status = "all", int pageNumber = 1)
        {
            int pageSize = 10; // Số lượng cuộc hẹn dịch vụ trên mỗi trang

            // Lấy danh sách cuộc hẹn dịch vụ
            var appointmentsQuery = dc.Orders.Include(o => o.PidNavigation)
                .Include(o => o.Option)
                    .ThenInclude(opt => opt.DidNavigation)
                    .ThenInclude(d => d.Specialty)
                .AsQueryable();

            // Thực hiện tìm kiếm theo tên dịch vụ
            if (!string.IsNullOrEmpty(search_service))
            {
                appointmentsQuery = appointmentsQuery
                    .Where(o => o.Option.DidNavigation.Specialty != null &&
                                o.Option.DidNavigation.Specialty.SpecialtyName != null &&
                                o.Option.DidNavigation.Specialty.SpecialtyName.Contains(search_service.Trim()));
            }
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                appointmentsQuery = appointmentsQuery.Where(o => o.Status == status);
            }

            // Chọn các trường cần thiết và chuyển đổi sang ServiceAppointmentModel
            var appointments = appointmentsQuery.Select(o => new ServiceAppointmentModel
            {
                AppointmentId = o.Oid,
                PatientName = o.PidNavigation.Name,
                DoctorName = o.Option.DidNavigation.Name,
                SpecialtyName = o.Option.DidNavigation.Specialty.SpecialtyName,
                AppointmentDate = o.Option.DateExam.HasValue ? o.Option.DateExam.Value : DateTime.MinValue,
                Status = o.Status
            })
            .Skip((pageNumber - 1) * pageSize) // Bỏ qua số lượng cuộc hẹn đã hiển thị ở các trang trước
            .Take(pageSize) // Lấy số lượng cuộc hẹn tương ứng với trang hiện tại
            .ToList();

            // Tính tổng số cuộc hẹn
            var totalAppointments = appointmentsQuery.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalAppointments / (double)pageSize); // Tổng số trang
            ViewBag.PageNumber = pageNumber; // Trang hiện tại

            // Giữ lại các tham số tìm kiếm và lọc để truyền vào view
            ViewBag.SearchService = search_service;
            ViewBag.Status = status;

            return View(appointments);
        }


        //-------------------------------------------------------------------------------------------------------------

        public IActionResult ServiceAppointDetail(string id)
        {
            // Lấy thông tin cuộc hẹn dựa trên ID (OId)
            var appointment = dc.Orders
                .Include(o => o.PidNavigation) // Thông tin bệnh nhân
                .Include(o => o.Option) // Thông tin dịch vụ
                    .ThenInclude(opt => opt.DidNavigation) // Thông tin bác sĩ
                .ThenInclude(d => d.Specialty) // Thông tin chuyên khoa
                .Where(o => o.Oid == id)
                .Select(o => new ServiceAppointmentDetailViewModel
                {
                    AppointmentId = o.Oid,
                    PatientName = o.PidNavigation.Name,
                    PatientPhone = o.PidNavigation.Phone,
                    PatientGender = o.PidNavigation.Gender,
                    PatientDOB = o.PidNavigation.Dob,
                    PatientImage = o.PidNavigation.PatientImg,
                    SpecialtyName = o.Option.DidNavigation.Specialty.SpecialtyName, // Tên chuyên khoa
                    SpecialtyImage = o.Option.DidNavigation.Specialty.SpecialtyImg, // Hình ảnh chuyên khoa
                    AppointmentDate = o.Option.DateExam.HasValue ? o.Option.DateExam.Value : DateTime.MinValue,
                    AppointmentTime = o.Option.DateExam.HasValue ? o.Option.DateExam.Value.ToString("hh:mm tt") : "N/A",
                    Status = o.Status,
                    Fee = o.Option.DidNavigation.Price ?? 0,
                    SupportingStaff = "Nguyễn Văn C", // Tên nhân viên hỗ trợ, có thể thay đổi sau
                    ConsultationInfo = o.Symptom // Thông tin tư vấn
                })
                .FirstOrDefault();

            // Nếu không tìm thấy cuộc hẹn, trả về NotFound hoặc một view tương tự
            if (appointment == null)
            {
                return NotFound();
            }

            // Truyền dữ liệu đến view
            return View(appointment);
        }

        //-------------------------------------------------------------------------------------------------------------

        public IActionResult Schedule()
        {

            var doctors = dc.Doctors.ToList();

            return View(doctors);
        }



        //-------------------------------------------------------------------------------------------------------------

        public IActionResult ContactList(string status = "all", int pageNumber = 1)
        {
            int pageSize = 10; // Số lượng liên hệ trên mỗi trang

            // Truy vấn danh sách liên hệ
            var contactsQuery = dc.Contacts.AsQueryable();

            // Lọc theo trạng thái nếu có
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                contactsQuery = contactsQuery.Where(c => c.Status == status);
            }

            // Đếm tổng số liên hệ
            var totalContacts = contactsQuery.Count();

            // Lấy danh sách liên hệ dựa trên trang hiện tại và kích thước trang
            var contacts = contactsQuery
                .Skip((pageNumber - 1) * pageSize)  // Bỏ qua các liên hệ của trang trước
                .Take(pageSize)  // Lấy số liên hệ tương ứng với trang hiện tại
                .Select(c => new ContactViewModel
                {
                    ContactId = c.ContactId,
                    FullName = c.Name,
                    Email = c.Email,
                    Description = c.Description,
                    Status = c.Status
                })
                .ToList();

            // Tính tổng số trang
            ViewBag.TotalPages = (int)Math.Ceiling(totalContacts / (double)pageSize);
            ViewBag.PageNumber = pageNumber; // Trang hiện tại
            ViewBag.Status = status; // Giữ lại trạng thái lọc để hiển thị

            return View(contacts);
        }

        //-------------------------------------------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
