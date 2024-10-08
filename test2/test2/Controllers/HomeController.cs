using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test2.Data;
using test2.Models;

namespace test2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DocCareContext dc;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,DocCareContext db)
        {
            _logger = logger;
            dc = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DoctorList()
        {
            var doctor = dc.Doctors.ToList();
            var specialities = dc.Specialties.ToList();

            ViewBag.Specialities = specialities;
            return View(doctor);
        }

        public IActionResult DoctorDetail()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult ForgotPass()
        {
            return View();
        }

        public IActionResult ServiceList()
        {
            var service = dc.Specialties.ToList();

            return View(service);
        }

        public IActionResult ServiceDetail()
        {
            return View();
        }

        public IActionResult Contact()
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
