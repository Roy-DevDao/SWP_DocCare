using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test2.Models;

namespace test2.Controllers
{
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;

        public StaffController(ILogger<StaffController> logger)
        {
            _logger = logger;
        }

        public IActionResult AppoitmentList()
        {
            return View();  // This will render /Views/Staff/AppoitmentList.cshtml
        }

        public IActionResult AppointmentDetail()
        {
            return View();  // This will render /Views/Staff/AppointmentDetail.cshtml
        }

        public IActionResult ServiceAppointList()
        {
            return View();  // This will render /Views/Staff/ServiceAppointList.cshtml
        }

        public IActionResult ServiceAppointDetail()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult Schedule()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult ContactList()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
