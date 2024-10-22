using test2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;
using test2.Data;
using Microsoft.AspNetCore.Authorization;

namespace test2.Controllers
{
    [Authorize(Roles = "0")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DocCareContext _context;

        public AdminController(ILogger<AdminController> logger, DocCareContext context)
        {
            _logger = logger;
            _context = context; // Kh?i t?o DbContext
        }

        public IActionResult Index()
        {
            var patients = _context.Patients.Count();
            var doctors = _context.Doctors.Count();
            var specialties = _context.Specialties.Count();
            var avgCost = _context.Doctors.Average(d => d.Price);
            var appointments = _context.Orders.Count();
            var service = _context.Specialties.Count();
            var feedback2 = _context.Feedbacks.Count();

            var feedback = _context.Feedbacks.OrderByDescending(f => f.DateCmt).Take(3).ToList();

            // pass data
            ViewBag.TotalPatients = patients;
            ViewBag.TotalDoctors = doctors;
            ViewBag.TotalSpecialties = specialties;
            ViewBag.AvgCost = avgCost;
            ViewBag.TotalAppointments = appointments;
            ViewBag.TotalService = service;
            ViewBag.TotalFeedback = feedback2;
            return View(feedback);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult ManageDoctor(int page = 1)
        //{
        //    int pageSize = 20;

        //    // Lọc các bác sĩ có đầy đủ thông tin
        //    var filteredDoctors = _context.Doctors
        //                                 .Where(d => d.Did != null
        //                                          && d.DoctorImg != null
        //                                          && d.Name != null
        //                                          && d.Gender != null
        //                                          && d.Position != null
        //                                          && d.Price != null);

        //    // Đếm tổng số bác sĩ đã lọc
        //    var totalDoctors = filteredDoctors.Count();

        //    // Tính tổng số trang
        //    ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
        //    ViewBag.CurrentPage = page;

        //    // Lấy danh sách bác sĩ cho trang hiện tại
        //    var doctors = filteredDoctors
        //                 .Skip((page - 1) * pageSize)
        //                 .Take(pageSize)
        //                 .Include(d => d.Specialty)
        //                 .ToList();

        //    return View(doctors);
        //}

        public IActionResult ManageDoctor(int page = 1, string sortPrice = "", string sortId = "", string sortGender = "")
        {
            int pageSize = 10;
            var filterDoctors = _context.Doctors.Where(doctor => doctor.Did != null && doctor.DoctorImg != null && doctor.Name != null && doctor.Gender != null && doctor.Position != null && doctor.Price != null);
            // Lọc theo giới tính
            if (!string.IsNullOrEmpty(sortGender))
            {
                filterDoctors = filterDoctors.Where(d => d.Gender == sortGender);
            }

            // Lọc theo giá
            if (!string.IsNullOrEmpty(sortPrice))
            {
                switch (sortPrice)
                {
                    case "100-200":
                        filterDoctors = filterDoctors.Where(d => d.Price >= 100 && d.Price <= 200);
                        break;
                    case "200-400":
                        filterDoctors = filterDoctors.Where(d => d.Price > 200 && d.Price <= 400);
                        break;
                    case "400+":
                        filterDoctors = filterDoctors.Where(d => d.Price > 400);
                        break;
                }
            }

            // Sắp xếp theo ID
            if (!string.IsNullOrEmpty(sortId))
            {
                if (sortId == "Increase")
                    filterDoctors = filterDoctors.OrderBy(d => d.Did);
                else if (sortId == "Decrease")
                    filterDoctors = filterDoctors.OrderByDescending(d => d.Did);
            }

            // Đếm tổng số bác sĩ đã lọc
            var totalDoctors = filterDoctors.Count();

            // Tính tổng số trang
            ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
            ViewBag.CurrentPage = page;

            // Lấy danh sách bác sĩ cho trang hiện tại
            var doctors = filterDoctors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(doctors);
        }
        public IActionResult DoctorDetails(string id)
        {
            // Fetch the doctor with the given ID from the database
            var doctorWithId = _context.Doctors.FirstOrDefault(d => d.Did.Equals(id));

            // If no doctor is found, return a "Not Found" view or redirect to an error page
            if (doctorWithId == null)
            {
                return NotFound(); // You can customize this to redirect or show a custom view
            }

            // Pass the doctor data to the view
            return View(doctorWithId);
        }


        public IActionResult AddDoctor()
        {
            return View();
        }

        // patient
        public IActionResult ManagePatient(int page = 1)
        {
            int pageSize = 10;
            var filterPatients = _context.Patients.Where(patient => patient.Pid != null && patient.Name != null && patient.Gender != null
                && patient.Dob != null && patient.Phone != null);

            var totalPatients = filterPatients.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalPatients / (double)pageSize);
            ViewBag.CurrentPage = page;
            var patients = filterPatients.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(patients);
        }



        public IActionResult PatientDetails(string id)
        {
            // Fetch the doctor with the given ID from the database
            var patientWithId = _context.Patients.FirstOrDefault(d => d.Pid.Equals(id));

            // If no doctor is found, return a "Not Found" view or redirect to an error page
            if (patientWithId == null)
            {
                return NotFound(); // You can customize this to redirect or show a custom view
            }

            // Pass the doctor data to the view
            return View(patientWithId);
        }

        public IActionResult AddPatient()
        {
            return View();
        }

        // service
        //public IActionResult ManageService(int page = 1, string sortPrice = "")
        //{
        //    int pageSize = 10;

        //    // Lấy tất cả các specialties bao gồm Doctors
        //    var specialties = _context.Specialties.Include(s => s.Doctors).AsQueryable();

        //    // Lọc theo giá nếu sortPrice không rỗng
        //    if (!string.IsNullOrEmpty(sortPrice))
        //    {
        //        switch (sortPrice)
        //        {
        //            case "Low":
        //                specialties = specialties.Where(s => s.Doctors.Any(d => d.Price >= 1 && d.Price <= 200));
        //                break;
        //            case "Medium":
        //                specialties = specialties.Where(s => s.Doctors.Any(d => d.Price > 200 && d.Price <= 400));
        //                break;
        //            case "High":
        //                specialties = specialties.Where(s => s.Doctors.Any(d => d.Price > 400 && d.Price <= 500));
        //                break;
        //        }
        //    }

        //    var totalService = specialties.Count();

        //    ViewBag.TotalPages = (int)Math.Ceiling(totalService / (double)pageSize);
        //    ViewBag.CurrentPage = page;

        //    var service = specialties.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    return View(service);
        //}

        public IActionResult ManageService(int page = 1)
        {
            int pageSize = 10;
            var specialties = _context.Specialties
    .Include(s => s.Doctors)  // Eager loading Doctors
    .ToList();
            var totalService = specialties.Count();

            ViewBag.TotalPages = (int)Math.Ceiling(specialties.Count / (double)pageSize);
            ViewBag.CurrentPage = page;

            var service = specialties.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(service);
        }

        public IActionResult ServiceDetails(string id)
        {
            // Fetch the doctor with the given ID from the database
            var serviceId = _context.Specialties.FirstOrDefault(d => d.SpecialtyId.Equals(id));

            // If no doctor is found, return a "Not Found" view or redirect to an error page
            if (serviceId == null)
            {
                return NotFound(); // You can customize this to redirect or show a custom view
            }

            // Pass the doctor data to the view
            return View(serviceId);
        }

        public IActionResult AddService()
        {
            return View();
        }

        public IActionResult EditService()
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
