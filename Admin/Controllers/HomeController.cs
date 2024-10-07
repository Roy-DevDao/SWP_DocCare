using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Admin.Controllers
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

		// doctor
		public IActionResult ManageDoctor()
        {
            return View();
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
            return View();
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
            return View();
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
