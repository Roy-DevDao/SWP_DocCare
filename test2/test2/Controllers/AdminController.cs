using test2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;
using test2.Data;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace test2.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DocCareContext _context;

        private Tuple<String, int> SortTid(string id)
        {
            var match = Regex.Match(id, @"([a-zA-Z]+)(\d+)");
            if(match.Success)
            {
                var letters = match.Groups[1].Value;
                var numbers = int.Parse(match.Groups[2].Value);
                return Tuple.Create(letters, numbers);
            }    
            return Tuple.Create(id, 0);
        }

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

        //public IActionResult ManageDoctor(string sortColumn = "Did", string sortDirection = "asc",  int page = 1, string sortPrice = "", string sortId = "", string sortName = "", string sortGender = "", string searchId = "")
        //{
        //    int pageSize = 20;

        //    // Lọc danh sách doctors từ cơ sở dữ liệu
        //    var filterDoctors = _context.Doctors.Where(doctor => doctor.Did != null && doctor.DoctorImg != null && doctor.Name != null && doctor.Gender != null && doctor.Position != null && doctor.Price != null);


        //    switch (sortColumn)
        //    {
        //        case "Name":
        //            filterDoctors = (sortDirection == "asc") ? filterDoctors.OrderBy(d => d.Name) : filterDoctors.OrderByDescending(d => d.Name);
        //            break;
        //        case "Fee":
        //            filterDoctors = (sortDirection == "asc") ? filterDoctors.OrderBy(d => d.Price) : filterDoctors.OrderByDescending(d => d.Price);
        //            break;

        //        default:
        //            filterDoctors = (sortDirection == "asc") ? filterDoctors.OrderBy(d => SortTid(d.Did)) : filterDoctors.OrderByDescending(d => SortTid(d.Did));
        //            break;
        //    }
        //    // Tìm kiếm theo ID
        //    if (!string.IsNullOrEmpty(searchId))
        //    {
        //        filterDoctors = filterDoctors.Where(d => d.Did.Contains(searchId));
        //    }

        //    // Lọc theo giới tính
        //    if (!string.IsNullOrEmpty(sortGender))
        //    {
        //        filterDoctors = filterDoctors.Where(d => d.Gender == sortGender);
        //    }

        //    // Lọc theo giá (Nếu có)
        //    if (!string.IsNullOrEmpty(sortPrice))
        //    {
        //        switch (sortPrice)
        //        {
        //            case "100-200":
        //                filterDoctors = filterDoctors.Where(d => d.Price >= 100 && d.Price <= 200);
        //                break;
        //            case "200-400":
        //                filterDoctors = filterDoctors.Where(d => d.Price > 200 && d.Price <= 400);
        //                break;
        //            case "400+":
        //                filterDoctors = filterDoctors.Where(d => d.Price > 400);
        //                break;
        //        }
        //    }

        //    // Lấy danh sách kết quả đã lọc
        //    var doctorList = filterDoctors.ToList();

        //    // Sắp xếp theo ID
        //    if (!string.IsNullOrEmpty(sortId))
        //    {
        //        if (sortId == "Increase")
        //            doctorList = doctorList.OrderBy(d => SortTid(d.Did)).ToList();
        //        else if (sortId == "Decrease")
        //            doctorList = doctorList.OrderByDescending(d => SortTid(d.Did)).ToList();
        //    }

        //    // Sắp xếp theo Name
        //    if (!string.IsNullOrEmpty(sortName))
        //    {
        //        if (sortName == "Inx    crease")
        //            doctorList = doctorList.OrderBy(d => d.Name).ToList();
        //        else if (sortName == "Decrease")
        //            doctorList = doctorList.OrderByDescending(d => d.Name).ToList();
        //    }

        //    // Sắp xếp theo Price
        //    if (!string.IsNullOrEmpty(sortPrice))
        //    {
        //        if (sortPrice == "Increase")
        //            doctorList = doctorList.OrderBy(d => d.Price).ToList();
        //        else if (sortPrice == "Decrease")
        //            doctorList = doctorList.OrderByDescending(d => d.Price).ToList();
        //    }

        //    // Tính tổng số lượng doctors sau khi lọc
        //    var totalDoctors = doctorList.Count();

        //    // Lưu trạng thái sắp xếp hiện tại vào ViewBag để hiển thị trong view
        //    ViewBag.CurrentSortPrice = sortPrice;
        //    ViewBag.CurrentSortName = sortName;
        //    ViewBag.CurrentSortId = sortId;
        //    ViewBag.CurrentSortGender = sortGender;
        //    ViewBag.CurrentSearchId = searchId;
        //    ViewBag.SortColumn = sortColumn;
        //    ViewBag.SortDirection = sortDirection;

        //    // Phân trang kết quả
        //    ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
        //    ViewBag.CurrentPage = page;
        //    var doctors = doctorList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    // Tính toán số lượng kết quả hiển thị
        //    int startResult = (page - 1) * pageSize + 1;
        //    int endResult = startResult + doctors.Count - 1;

        //    // Đưa thông tin phân trang vào ViewBag
        //    ViewBag.StartResult = startResult;
        //    ViewBag.EndResult = endResult;
        //    ViewBag.TotalDoctors = totalDoctors;

        //    // Trả về view với danh sách doctors
        //    return View(doctors);
        //}

        public IActionResult ManageDoctor(string sortColumn = "Did", string sortDirection = "asc", int page = 1, string sortPrice = "", string sortId = "", string sortName = "", string sortGender = "", string searchId = "")
        {
            int pageSize = 20;

            // Lọc danh sách doctors từ cơ sở dữ liệu
            var filterDoctors = _context.Doctors.Where(doctor => doctor.Did != null && doctor.DoctorImg != null && doctor.Name != null && doctor.Gender != null && doctor.Position != null && doctor.Price != null);

            // Tìm kiếm theo ID
            if (!string.IsNullOrEmpty(searchId))
            {
                filterDoctors = filterDoctors.Where(d => d.Did.Contains(searchId));
            }

            // Lọc theo giới tính
            if (!string.IsNullOrEmpty(sortGender))
            {
                filterDoctors = filterDoctors.Where(d => d.Gender == sortGender);
            }

            // Lọc theo giá (Nếu có)
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

            // Lấy danh sách kết quả đã lọc
            var doctorList = filterDoctors.ToList();

            // Sắp xếp theo cột
            switch (sortColumn)
            {
                case "Name":
                    doctorList = (sortDirection == "asc") ? doctorList.OrderBy(d => d.Name).ToList() : doctorList.OrderByDescending(d => d.Name).ToList();
                    break;
                case "Fee":
                    doctorList = (sortDirection == "asc") ? doctorList.OrderBy(d => d.Price).ToList() : doctorList.OrderByDescending(d => d.Price).ToList();
                    break;
                default:
                    doctorList = (sortDirection == "asc") ? doctorList.OrderBy(d => SortTid(d.Did)).ToList() : doctorList.OrderByDescending(d => SortTid(d.Did)).ToList();
                    break;
            }

            // Sắp xếp theo ID
            if (!string.IsNullOrEmpty(sortId))
            {
                if (sortId == "Increase")
                    doctorList = doctorList.OrderBy(d => SortTid(d.Did)).ToList();
                else if (sortId == "Decrease")
                    doctorList = doctorList.OrderByDescending(d => SortTid(d.Did)).ToList();
            }

            // Sắp xếp theo Name
            if (!string.IsNullOrEmpty(sortName))
            {
                if (sortName == "Increase")
                    doctorList = doctorList.OrderBy(d => d.Name).ToList();
                else if (sortName == "Decrease")
                    doctorList = doctorList.OrderByDescending(d => d.Name).ToList();
            }

            // Sắp xếp theo Price
            if (!string.IsNullOrEmpty(sortPrice))
            {
                if (sortPrice == "Increase")
                    doctorList = doctorList.OrderBy(d => d.Price).ToList();
                else if (sortPrice == "Decrease")
                    doctorList = doctorList.OrderByDescending(d => d.Price).ToList();
            }

            // Tính tổng số lượng doctors sau khi lọc
            var totalDoctors = doctorList.Count();

            // Lưu trạng thái sắp xếp hiện tại vào ViewBag để hiển thị trong view
            ViewBag.CurrentSortPrice = sortPrice;
            ViewBag.CurrentSortName = sortName;
            ViewBag.CurrentSortId = sortId;
            ViewBag.CurrentSortGender = sortGender;
            ViewBag.CurrentSearchId = searchId;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortDirection = sortDirection;

            // Phân trang kết quả
            ViewBag.TotalPages = (int)Math.Ceiling(totalDoctors / (double)pageSize);
            ViewBag.CurrentPage = page;
            var doctors = doctorList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Tính toán số lượng kết quả hiển thị
            int startResult = (page - 1) * pageSize + 1;
            int endResult = startResult + doctors.Count - 1;

            // Đưa thông tin phân trang vào ViewBag
            ViewBag.StartResult = startResult;
            ViewBag.EndResult = endResult;
            ViewBag.TotalDoctors = totalDoctors;

            // Trả về view với danh sách doctors
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


        //// GET: Add Doctor
        //public IActionResult AddDoctor()
        //{
        //    var specialties = _context.Specialties.Select(d => new { d.SpecialtyId, d.SpecialtyName }).ToList();

        //    ViewBag.Specialties = specialties;

        //    return View();
        //}

        //// POST: Add Doctor
        //[HttpPost]
        //public IActionResult AddDoctor(Doctor model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Create a new Account and assign its ID to Doctor's Did (Doctor ID)
        //        var newAccount = new Account
        //        {
        //            Id = model.Did, // Doctor's ID will be linked with Account's ID
        //            Username = model.Name,
        //            // Add additional fields if required like Password, Email, etc.
        //        };
        //        _context.Accounts.Add(newAccount);

        //        // Add Doctor to database
        //        _context.Doctors.Add(model);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    // Reload ViewBag data in case of validation errors
        //    ViewBag.Specialties = _context.Specialties.Select(d => new { d.SpecialtyId, d.SpecialtyName }).ToList();

        //    return View(model);
        //}

        public IActionResult AddDoctor()
        {
            // Fetch the list of specialties from the Specialty table
            var specialties = _context.Specialties.ToList();

            // Pass the specialties to the view using ViewBag
            ViewBag.Specialties = new SelectList(specialties, "SpecialtyId", "SpecialtyId");

            return View();
        }


        [HttpPost]
        public IActionResult AddDoctor(Doctor newDoctor, string AccountId)
        {
            try
            {
                // Check if AccountId exists in the Accounts table
                var account = _context.Accounts.FirstOrDefault(a => a.Id == AccountId);

                if (account == null)
                {
                    // If account doesn't exist, add an error message and return the view
                    ModelState.AddModelError("AccountId", "The provided Account ID does not exist.");
                    return View(newDoctor); // Return view to show error
                }

                // Set the DidNavigation to the found account
                newDoctor.DidNavigation = account;  // Ensuring DidNavigation is correctly set

                // Assign the Did (Doctor ID) to match the Account ID
                newDoctor.Did = account.Id; // Ensure Did matches AccountId

                // Now validate the model after assigning the Account
                if (ModelState.IsValid)
                {
                    _context.Doctors.Add(newDoctor); // Add new doctor to the database
                    _context.SaveChanges();
                    return RedirectToAction("ManageDoctor");
                }
            }
            catch (Exception ex)
            {
                // Catch any errors and display them, including inner exception details
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError(string.Empty, $"An error occurred: {errorMessage}");
            }

            // If any errors, return the view with the current doctor data and errors
            return View(newDoctor);
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

            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);

            _context.SaveChanges();

            return RedirectToAction("ManageDoctor");
        }

        public IActionResult ManagePatient( string sortColumn= "Pid",string sortDirection = "asc", int page = 1, string sortID = "", string sortName = "", string sortDOB = "", string sortGender = "", string searchId = "")
        {
            int pageSize = 20;

            // Lọc danh sách bệnh nhân từ cơ sở dữ liệu
            var filterPatients = _context.Patients.Where(patient => patient.Pid != null && patient.Name != null && patient.Gender != null
                && patient.Dob != null && patient.Phone != null);

            switch (sortColumn)
            {
                case "PDate":
                    filterPatients = (sortDirection == "asc") ? filterPatients.OrderBy(o => o.Dob) : filterPatients.OrderByDescending(o => o.Dob);
                    break;
                case "PName":
                    filterPatients = (sortDirection == "asc") ? filterPatients.OrderBy(o => o.Name) : filterPatients.OrderByDescending(o => o.Name);
                    break;
                default: // "Pid" as default
                    filterPatients = (sortDirection == "asc") ? filterPatients.OrderBy(o => o.Pid) : filterPatients.OrderByDescending(o => o.Pid);
                    break;
            }

            // Tìm kiếm theo ID
            if (!string.IsNullOrEmpty(searchId))
            {
                filterPatients = filterPatients.Where(p => p.Pid.Contains(searchId));
            }

            // Lọc theo giới tính
            if (!string.IsNullOrEmpty(sortGender))
            {
                filterPatients = filterPatients.Where(p => p.Gender == sortGender);
            }

            // Lấy danh sách kết quả đã lọc
            var patientList = filterPatients.ToList();

            // Sắp xếp theo ID
            if (!string.IsNullOrEmpty(sortID))
            {
                if (sortID == "Increase")
                    patientList = patientList.OrderBy(p => SortTid(p.Pid)).ToList();
                else if (sortID == "Decrease")
                    patientList = patientList.OrderByDescending(p => SortTid(p.Pid)).ToList();
            }

            // Sắp xếp theo Name
            if (!string.IsNullOrEmpty(sortName))
            {
                if (sortName == "Increase")
                    patientList = patientList.OrderBy(p => p.Name).ToList();
                else if (sortName == "Decrease")
                    patientList = patientList.OrderByDescending(p => p.Name).ToList();
            }

            // Sắp xếp theo DOB
            if (!string.IsNullOrEmpty(sortDOB))
            {
                if (sortDOB == "Increase")
                    patientList = patientList.OrderBy(p => p.Dob).ToList();
                else if (sortDOB == "Decrease")
                    patientList = patientList.OrderByDescending(p => p.Dob).ToList();
            }

            // Tính tổng số bệnh nhân sau khi lọc
            var totalPatients = patientList.Count();

            // Lưu trạng thái sắp xếp hiện tại vào ViewBag để hiển thị trong view
            ViewBag.CurrentSortID = sortID;
            ViewBag.CurrentSortName = sortName;
            ViewBag.CurrentSortDOB = sortDOB;
            ViewBag.CurrentSortGender = sortGender;
            ViewBag.CurrentSearchId = searchId;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortDirection = sortDirection;

            // Phân trang kết quả
            ViewBag.TotalPages = (int)Math.Ceiling(totalPatients / (double)pageSize);
            ViewBag.CurrentPage = page;
            var patients = patientList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Tính toán số lượng kết quả hiển thị
            int startResult = (page - 1) * pageSize + 1;
            int endResult = startResult + patients.Count - 1;

            // Đưa thông tin phân trang vào ViewBag
            ViewBag.StartResult = startResult;
            ViewBag.EndResult = endResult;
            ViewBag.TotalPatients = totalPatients;

            // Trả về view với danh sách bệnh nhân
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
            var patientWithId = _context.Patients
                .Include(p => p.HealthRecords)
                .Include(p => p.Feedbacks)
                .FirstOrDefault(d => d.Pid.Equals(id));

            if (patientWithId == null)
            {
                return NotFound();
            }

            // Tổng số dịch vụ đã sử dụng (số lượng HealthRecord của bệnh nhân)
            int totalServicesUsed = patientWithId.HealthRecords.Count();

            // Tổng số phản hồi (số lượng Feedback của bệnh nhân)
            int totalFeedbacks = patientWithId.Feedbacks.Count();

            // Đánh giá trung bình
            double averageRating = patientWithId.Feedbacks.Any()
                ? patientWithId.Feedbacks.Average(f => f.Star ?? 0)
                : 0;

            // Số lượng phản hồi theo từng sao (5, 4, 3, 2, 1)
            var feedbackStats = patientWithId.Feedbacks
                .Where(f => f.Star.HasValue)
                .GroupBy(f => f.Star)
                .ToDictionary(g => g.Key, g => g.Count());
            int totalAvailableServices = 28;
            double serviceUsagePercentage = ((double)totalServicesUsed / totalAvailableServices) * 100;

            ViewData["TotalServicesUsed"] = totalServicesUsed;
            ViewData["ServiceUsagePercentage"] = serviceUsagePercentage;
            ViewData["TotalFeedback"] = totalFeedbacks;
            ViewData["AverageRating"] = averageRating.ToString("0.0"); // Định dạng với 1 chữ số thập phân
            ViewData["FeedbackStats"] = feedbackStats;

            return View(patientWithId);
        }






        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPatient(Patient newPatient, string AccountId)
        {
            try
            {
                // Check if AccountId exists in the Accounts table
                var account = _context.Accounts.FirstOrDefault(a => a.Id == AccountId);

                if (account == null)
                {
                    // If the account doesn't exist, add an error message and return the view
                    ModelState.AddModelError("AccountId", "The provided Account ID does not exist.");
                    return View(newPatient); // Return the view to show the error
                }

                // Set the PidNavigation to the found account
                newPatient.PidNavigation = account;

                // Assign the Pid (Patient ID) to match the Account ID
                newPatient.Pid = account.Id; // This ensures Pid matches AccountId

                // Now validate the model after assigning the Account
                if (ModelState.IsValid)
                {
                    _context.Patients.Add(newPatient); // Add new patient to the database
                    _context.SaveChanges();
                    return RedirectToAction("ManagePatient");
                }
            }
            catch (Exception ex)
            {
                // Catch any errors and display them
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            // If any errors, return the view with the current patient data and errors
            return View(newPatient);
        }









        public IActionResult ManageService(string sortColumn = "ID", string sortDirection = "asc", int page = 1, string sortPrice = "", string sortID = "", string sortName = "", string searchId = "")
        {
            int pageSize = 10;

            var filterSpecialties = _context.Specialties
                                             .Include(s => s.Doctors) // Bao gồm danh sách Doctors
                                             .Where(s => s.SpecialtyId != null
                                                      && s.SpecialtyName != null
                                                      && s.LongDescription != null
                                                      && s.ShortDescription != null)
                                             .AsQueryable();


            switch (sortColumn)
            {
                case "Name":
                    filterSpecialties = (sortDirection == "asc") ? filterSpecialties.OrderBy(s => s.SpecialtyName) : filterSpecialties.OrderByDescending(s => s.SpecialtyName);
                    break;
                case "Price":
                    filterSpecialties = (sortDirection == "asc") ? filterSpecialties.OrderBy(s => s.Doctors.Min(d => d.Price)) : filterSpecialties.OrderByDescending(s => s.Doctors.Max(d => d.Price));
                    break;
                default:
                    filterSpecialties = (sortDirection == "asc") ? filterSpecialties.OrderBy(s => s.SpecialtyId) : filterSpecialties.OrderByDescending(s => s.SpecialtyId);
                    break;
            }
            // Tìm kiếm theo ID
            if (!string.IsNullOrEmpty(searchId))
            {
                filterSpecialties = filterSpecialties.Where(s => s.SpecialtyId.Contains(searchId));
            }

            // Lọc theo giá
            if (!string.IsNullOrEmpty(sortPrice))
            {
                switch (sortPrice)
                {
                    case "1-200":
                        filterSpecialties = filterSpecialties.Where(s => s.Doctors.Any(d => d.Price >= 1 && d.Price <= 200));
                        break;
                    case "200-400":
                        filterSpecialties = filterSpecialties.Where(s => s.Doctors.Any(d => d.Price > 200 && d.Price <= 400));
                        break;
                    case "400-500":
                        filterSpecialties = filterSpecialties.Where(s => s.Doctors.Any(d => d.Price > 400));
                        break;
                }
            }

            var serviceList = filterSpecialties.ToList();

            // Sắp xếp theo SpecialtyId (ID)
            if (!string.IsNullOrEmpty(sortID))
            {
                if (sortID == "Increase")
                    serviceList = serviceList.OrderBy(s => SortTid(s.SpecialtyId)).ToList();
                else if (sortID == "Decrease")
                    serviceList = serviceList.OrderByDescending(s => SortTid(s.SpecialtyId)).ToList();
            }

            // Sắp xếp theo SpecialtyName (Name)
            if (!string.IsNullOrEmpty(sortName))
            {
                if (sortName == "Increase")
                    serviceList = serviceList.OrderBy(s => s.SpecialtyName).ToList();
                else if (sortName == "Decrease")
                    serviceList = serviceList.OrderByDescending(s => s.SpecialtyName).ToList();
            }

            // Sắp xếp theo giá (Price)
            if (!string.IsNullOrEmpty(sortPrice))
            {
                if (sortPrice == "Increase")
                    serviceList = serviceList.OrderBy(s => s.Doctors.Min(d => d.Price)).ToList();
                else if (sortPrice == "Decrease")
                    serviceList = serviceList.OrderByDescending(s => s.Doctors.Max(d => d.Price)).ToList();
            }

            // Tổng số dịch vụ
            var totalService = serviceList.Count();

            // Tính toán phân trang
            ViewBag.TotalPages = (int)Math.Ceiling(totalService / (double)pageSize);
            ViewBag.CurrentPage = page;
            var service = serviceList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Lưu trạng thái sắp xếp hiện tại vào ViewBag để hiển thị trong view
            ViewBag.SortPrice = sortPrice;
            ViewBag.SortID = sortID;
            ViewBag.SortName = sortName;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortDirection = sortDirection;

            // Kết quả hiển thị
            int startResult = (page - 1) * pageSize + 1;
            int endResult = startResult + service.Count - 1;

            ViewBag.StartResult = startResult;
            ViewBag.EndResult = endResult;
            ViewBag.TotalServices = totalService;

            return View(service);
        }

        //public IActionResult ManageService(string sortColumn = "ID", string sortDirection = "asc", int page = 1, string sortPrice = "", string searchId = "")
        //{
        //    int pageSize = 10;

        //    // Fetch and filter specialties from the database
        //    var filterSpecialties = _context.Specialties
        //                                    .Include(s => s.Doctors)
        //                                    .Where(s => s.SpecialtyId != null
        //                                             && s.SpecialtyName != null
        //                                             && s.LongDescription != null
        //                                             && s.ShortDescription != null)
        //                                    .AsQueryable();

        //    // Handle Sorting
        //    switch (sortColumn)
        //    {
        //        case "Name":
        //            filterSpecialties = (sortDirection == "asc") ? filterSpecialties.OrderBy(s => s.SpecialtyName) : filterSpecialties.OrderByDescending(s => s.SpecialtyName);
        //            break;
        //        case "Price":
        //            filterSpecialties = (sortDirection == "asc") ? filterSpecialties.OrderBy(s => s.Doctors.Min(d => d.Price)) : filterSpecialties.OrderByDescending(s => s.Doctors.Max(d => d.Price));
        //            break;
        //        default:
        //            filterSpecialties = (sortDirection == "asc") ? filterSpecialties.OrderBy(s => s.SpecialtyId) : filterSpecialties.OrderByDescending(s => s.SpecialtyId);
        //            break;
        //    }

        //    // Search by ID (if applicable)
        //    if (!string.IsNullOrEmpty(searchId))
        //    {
        //        filterSpecialties = filterSpecialties.Where(s => s.SpecialtyId.Contains(searchId));
        //    }

        //    // Apply sorting by price range (if applicable)
        //    if (!string.IsNullOrEmpty(sortPrice))
        //    {
        //        switch (sortPrice)
        //        {
        //            case "1-200":
        //                filterSpecialties = filterSpecialties.Where(s => s.Doctors.Any(d => d.Price >= 1 && d.Price <= 200));
        //                break;
        //            case "200-400":
        //                filterSpecialties = filterSpecialties.Where(s => s.Doctors.Any(d => d.Price > 200 && d.Price <= 400));
        //                break;
        //            case "400-500":
        //                filterSpecialties = filterSpecialties.Where(s => s.Doctors.Any(d => d.Price > 400));
        //                break;
        //        }
        //    }

        //    // Paginate the results
        //    var serviceList = filterSpecialties.ToList();
        //    var totalService = serviceList.Count();
        //    var service = serviceList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    // Calculate pagination details
        //    ViewBag.TotalPages = (int)Math.Ceiling(totalService / (double)pageSize);
        //    ViewBag.CurrentPage = page;

        //    // Pass sorting details back to the view
        //    ViewBag.SortColumn = sortColumn;
        //    ViewBag.SortDirection = sortDirection;
        //    ViewBag.SortPrice = sortPrice;

        //    // Results range for current page
        //    int startResult = (page - 1) * pageSize + 1;
        //    int endResult = startResult + service.Count - 1;

        //    ViewBag.StartResult = startResult;
        //    ViewBag.EndResult = endResult;
        //    ViewBag.TotalServices = totalService;

        //    return View(service);
        //}
        //public  IActionResult ManageService(string sortColumn = "ID", string sortDirection = "asc", string searchId = "", int page = 1)
        //{
        //    // Truy vấn ban đầu
        //    var specialties = _context.Specialties.Include(s => s.Doctors).AsQueryable();

        //    // Thực hiện lọc theo `searchId` nếu có
        //    if (!string.IsNullOrEmpty(searchId))
        //    {
        //        specialties = specialties.Where(s => s.SpecialtyId.Contains(searchId));
        //    }

        //    // Thực hiện sắp xếp theo `sortColumn` và `sortDirection`
        //    specialties = sortColumn switch
        //    {
        //        "Name" => sortDirection == "asc" ? specialties.OrderBy(s => s.SpecialtyName) : specialties.OrderByDescending(s => s.SpecialtyName),
        //        "Price" => sortDirection == "asc" ? specialties.OrderBy(s => s.Doctors.Min(d => d.Price)) : specialties.OrderByDescending(s => s.Doctors.Max(d => d.Price)),
        //        _ => sortDirection == "asc" ? specialties.OrderBy(s => s.SpecialtyId) : specialties.OrderByDescending(s => s.SpecialtyId)
        //    };

        //    // Thực hiện phân trang (pagination) và các xử lý khác nếu cần

        //    // Lưu trữ trạng thái sort vào `ViewBag` để sử dụng ở view
        //    ViewBag.SortColumn = sortColumn;
        //    ViewBag.SortDirection = sortDirection;
        //    ViewBag.SearchId = searchId;

        //    // Trả về view với danh sách `specialties` đã được xử lý
        //    return View( specialties.ToListAsync());
        //}


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


        [HttpGet]
        public IActionResult EditService(string id)
        {
            var service = _context.Specialties.FirstOrDefault(s => s.SpecialtyId == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
        [HttpPost]
        public IActionResult EditService(Specialty model)
        {
            if (ModelState.IsValid)
            {
                var existingService = _context.Specialties.FirstOrDefault(s => s.SpecialtyId == model.SpecialtyId);

                if (existingService != null)
                {
                    existingService.SpecialtyName = model.SpecialtyName;
                    existingService.ShortDescription = model.ShortDescription;
                    existingService.LongDescription = model.LongDescription;
                    existingService.SpecialtyImg = model.SpecialtyImg;

                    _context.SaveChanges();

                    return RedirectToAction("ManageService");
                }
                else
                {
                    ModelState.AddModelError("", "Service not found.");
                }
            }

            return View(model);
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(Specialty newService)
        {
            var existingService = _context.Specialties
                .FirstOrDefault(s => s.SpecialtyId == newService.SpecialtyId || s.SpecialtyName == newService.SpecialtyName);

            if (existingService != null)
            {
                ModelState.AddModelError(string.Empty, "A service with the same ID or name already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Specialties.Add(newService);
                _context.SaveChanges();
                return RedirectToAction("ManageService");
            }

            return View(newService);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        
    }

    }

