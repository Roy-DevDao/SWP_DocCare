using DontCare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DontCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ViewAppointment()
        {
            return View();
        }
        public IActionResult ViewAppointmentDetail()
        {

            return View();
        }
        public IActionResult Feedback()
        {
            return View();
        }
        public IActionResult ViewPatient()
        {
            return View();
        }
        public IActionResult ViewPatientDetail()
        {
            return View();
        }
        public IActionResult AppointmentHistory()
        {
            return View();
        }
        public IActionResult BookingAppointment()
        {
            return View();
        }
        public IActionResult BookingService()
        {
            return View();
        }
        public IActionResult ServiceHistory()
        {
            return View();
        }
        public IActionResult Comment()
        {
            return View();
        }
        public IActionResult Payment()
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
