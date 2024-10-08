using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DocCareContext _context;

        public HomeController(ILogger<HomeController> logger, DocCareContext context)
        {
            _logger = logger;
            _context = context; // Kh?i t?o DbContext
        }

        public IActionResult Index()
        {
            var feedback = _context.Feedbacks.OrderByDescending(f => f.DateCmt).Take(3).ToList();
            return View(feedback);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // doctor
        public IActionResult ManageDoctor()
        {
            // L?y danh sách bác s? t? c? s? d? li?u
            var doctors = _context.Doctors.Include(d => d.Specialty).ToList();
            return View(doctors);
        }
        


        public IActionResult DoctorDetails()
        {
            return View();
        }

        public IActionResult AddDoctor()
        {
            return View();
        }

        // patient
        public IActionResult ManagePatient()
        {
            var patients = _context.Patients.ToList();
            return View(patients);
        }



        public IActionResult PatientDetails()
        {
            return View();
        }

        public IActionResult AddPatient()
        {
            return View();
        }

        // service
        public IActionResult ManageService()
        {
            var services = _context.Specialties.ToList();

            return View(services);
        }

        public IActionResult ServiceDetails()
        {
            return View();
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
