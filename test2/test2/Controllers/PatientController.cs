using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace test2.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        public IActionResult AppointmentHistory()
        {
            return View();  // This will render /Views/Staff/AppoitmentList.cshtml
        }

        public IActionResult BookingAppointment()
        {
            return View();  // This will render /Views/Staff/AppointmentDetail.cshtml
        }

        public IActionResult BookingService()
        {
            return View();  // This will render /Views/Staff/ServiceAppointList.cshtml
        }

        public IActionResult Payment()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult ServiceHistory()
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
