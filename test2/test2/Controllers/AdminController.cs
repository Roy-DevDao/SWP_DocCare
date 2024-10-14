using test2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;
using test2.Data;

namespace test2.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DocCareContext _context;

        public AdminController(ILogger<AdminController> logger, DocCareContext context)
        {
            _logger = logger;
            _context = context; 
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
        public IActionResult ManageDoctor(int page = 1, string sortPrice = "", string sortId = "", string sortGender = "", string searchId="")
        {
            int pageSize = 20;
            var filterDoctors = _context.Doctors.Where(doctor => doctor.Did != null && doctor.DoctorImg != null && doctor.Name != null && doctor.Gender != null && doctor.Position != null && doctor.Price != null);

            if (!string.IsNullOrEmpty(searchId))
            {
                filterDoctors = filterDoctors.Where(d => d.Did.Contains(searchId));
            }
            if (!string.IsNullOrEmpty(sortGender))
            {
                filterDoctors = filterDoctors.Where(d => d.Gender == sortGender);
            }

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

            if (!string.IsNullOrEmpty(sortId))
            {
                if (sortId == "Increase")
                    filterDoctors = filterDoctors.OrderBy(d => d.Did);
                else if (sortId == "Decrease")
                    filterDoctors = filterDoctors.OrderByDescending(d => d.Did);
            }

            var totalDoctors = filterDoctors.Count();

            ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.CurrentSortPrice = sortPrice;
            ViewBag.CurrentSortGender = sortGender;
            ViewBag.CurrentSortId = sortId;

            var doctors = filterDoctors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            int startResult = (page - 1) * pageSize + 1;
            int endResult = startResult + doctors.Count - 1;

            ViewBag.StartResult = startResult;
            ViewBag.EndResult = endResult;
            ViewBag.TotalDoctors = totalDoctors;

            return View(doctors);
        }


        public IActionResult DoctorDetails(string id)
        {
            var doctorWithId = _context.Doctors.Include(s => s.Feedbacks).FirstOrDefault(d => d.Did.Equals(id));

            if (doctorWithId == null)
            {
                return NotFound(); 
            }

            return View(doctorWithId);
        }


        public IActionResult AddDoctor()
        {
            var genders = new List<string> { "Male", "Female", "Other" };
            var specialties = _context.Specialties.Select(d => d.SpecialtyName).Distinct().ToList();
            var position = _context.Doctors.Select(d => d.Position).Distinct().ToList();

            ViewBag.Gender = genders;
            ViewBag.Specialties = specialties;
            ViewBag.Position = position;

            return View();
        }
        [HttpPost]
        public IActionResult AddDoctor(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction("ManageDoctor");
            }
            return View(doctor);
        }

        [HttpPost]
        public IActionResult DeleteDoctor(string id)
        {
            var options = _context.Options.Where(o => o.Did == id).ToList();
            var orders = _context.Orders.Where(order => options.Select(o => o.OptionId).Contains(order.OptionId)).ToList();
            var payments = _context.Payments.Where(p => orders.Select(o => o.Oid).Contains(p.Oid)).ToList();
            var feedbacks = _context.Feedbacks.Where(f => f.Did == id).ToList();
            var detailDoctors = _context.DetailDoctors.Where(d => d.Did == id).ToList();
            var healthRecords = _context.HealthRecords.Where(h => h.Did == id).ToList();
            var schedules = _context.Schedules.Where(s => s.Did == id).ToList();

            _context.HealthRecords.RemoveRange(healthRecords);
            _context.Feedbacks.RemoveRange(feedbacks);
            _context.DetailDoctors.RemoveRange(detailDoctors);
            _context.Payments.RemoveRange(payments);
            _context.Orders.RemoveRange(orders);
            _context.Options.RemoveRange(options);
            _context.Schedules.RemoveRange(schedules);

            // Tìm và xóa bác sĩ
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);

            // Lưu các thay đổi vào database
            _context.SaveChanges();

            return RedirectToAction("ManageDoctor");
        }




        // patient
        public IActionResult ManagePatient(int page = 1, string sortID = "", string sortGender = "", string searchId="")
        {
            int pageSize = 20;
            var filterPatients = _context.Patients.Where(patient => patient.Pid != null && patient.Name != null && patient.Gender != null
                && patient.Dob != null && patient.Phone != null);

            if (!string.IsNullOrEmpty(sortGender))
            {
                filterPatients = filterPatients.Where(p => p.Gender == sortGender);
            }

            if (!string.IsNullOrEmpty(sortID))
            {
                if (sortID == "Increase")
                {
                    filterPatients = filterPatients.OrderBy(p => p.Pid);
                }
                else if (sortID == "Decrease")
                {
                    filterPatients = filterPatients.OrderByDescending(p => p.Pid);
                }
            }

            if(!string.IsNullOrEmpty(searchId))
            {
                filterPatients = filterPatients.Where(p => p.Pid.Contains(searchId));
            }
            var totalPatients = filterPatients.Count();

            ViewBag.TotalPages = (int)Math.Ceiling(totalPatients / (double)pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.CurrentSortID = sortID;
            ViewBag.CurrentSortGender = sortGender;

            var patients = filterPatients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            int startResult = (page - 1) * pageSize + 1;
            int endResult = startResult + patients.Count - 1;

            ViewBag.StartResult = startResult;
            ViewBag.EndResult = endResult;
            ViewBag.TotalDoctors = totalPatients;

            return View(patients);
        }
        [HttpPost]
        public IActionResult DeletePatient(string id)
        {
            var feedbacks = _context.Feedbacks.Where(f => f.Pid == id).ToList();
            var healthRecords = _context.HealthRecords.Where(h => h.Pid == id).ToList();
            var orders = _context.Orders.Where(o => o.Pid == id).ToList();
            var account = _context.Accounts.Find(id);
            var payments = _context.Payments.Where(p => orders.Select(o => o.Oid).Contains(p.Oid)).ToList();

            _context.Feedbacks.RemoveRange(feedbacks);
            _context.HealthRecords.RemoveRange(healthRecords);
            _context.Orders.RemoveRange(orders);
            _context.Accounts.Remove(account);
            _context.Payments.RemoveRange(payments);

            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            _context.Patients.Remove(patient);
            _context.SaveChanges();

            return RedirectToAction("ManagePatient");
        }





        public IActionResult PatientDetails(string id)
            {
                var patientWithId = _context.Patients.FirstOrDefault(d => d.Pid.Equals(id));

                if (patientWithId == null)
                {
                    return NotFound();
                }

                return View(patientWithId);
            }

            public IActionResult AddPatient()
            {
                return View();
            }
        public IActionResult ManageService(int page = 1, string sortPrice = "", string sortID = "", string searchId="")
        {
            int pageSize = 10;

            var specialties = _context.Specialties.Include(s => s.Doctors).AsQueryable();

            if(!string.IsNullOrEmpty(searchId))
            {
                specialties = specialties.Where(s => s.SpecialtyId.Contains(searchId));
            }    
            if (!string.IsNullOrEmpty(sortPrice))
            {
                switch (sortPrice)
                {
                    case "1-200":
                        specialties = specialties.Where(s => s.Doctors.Any(d => d.Price >= 1 && d.Price <= 200))
                                                 .OrderBy(s => s.Doctors.Min(d => d.Price));
                        break;
                    case "200-400":
                        specialties = specialties.Where(s => s.Doctors.Any(d => d.Price > 200 && d.Price <= 400))
                                                 .OrderBy(s => s.Doctors.Min(d => d.Price));
                        break;
                    case "400-500":
                        specialties = specialties.Where(s => s.Doctors.Any(d => d.Price > 400))
                                                 .OrderBy(s => s.Doctors.Min(d => d.Price));
                        break;
                }
            }
            if (!string.IsNullOrEmpty(sortID))
            {
                if (sortID == "Increase")
                    specialties = specialties.OrderBy(s => s.SpecialtyId);
                else if (sortID == "Decrease")
                    specialties = specialties.OrderByDescending(s => s.SpecialtyId);
            }

            var totalService = specialties.Count();

            ViewBag.TotalPages = (int)Math.Ceiling(totalService / (double)pageSize);
            ViewBag.CurrentPage = page;

            var service = specialties.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.SortPrice = sortPrice;
            ViewBag.SortID = sortID;

            int startResult = (page - 1) * pageSize + 1;
            int endResult = startResult + service.Count - 1;

            ViewBag.StartResult = startResult;
            ViewBag.EndResult = endResult;
            ViewBag.TotalDoctors = totalService;

            return View(service);
        }
        [HttpPost]
        public IActionResult DeleteService(string id)
        {
            var doctors = _context.Doctors.Where(d => d.SpecialtyId == id).ToList();
            var feedbacks = _context.Feedbacks.Where(f => doctors.Select(d => d.Did).Contains(f.Did)).ToList();

            _context.Doctors.RemoveRange(doctors);
            _context.Feedbacks.RemoveRange(feedbacks);
            
           var service=_context.Specialties.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            _context.Specialties.Remove(service);
            _context.SaveChanges();

            return RedirectToAction("ManageService");

        }



        public IActionResult ServiceDetails(string id)
            {
                var serviceId = _context.Specialties.Include(s => s.Doctors).ThenInclude(d => d.Feedbacks).FirstOrDefault(d => d.SpecialtyId.Equals(id));

                if (serviceId == null)
                {
                    return NotFound(); 
                }
                var feedback = serviceId.Doctors.SelectMany(d => d.Feedbacks).ToList();
                var averageFeedback = feedback.Average(f => f.Star);

                ViewBag.AverageFeedback = averageFeedback;
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

