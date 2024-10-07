using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace test2.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Feedback()
        {
            return View();  // This will render /Views/Staff/AppoitmentList.cshtml
        }

        public IActionResult ViewAppointment()
        {
            return View();  // This will render /Views/Staff/AppointmentDetail.cshtml
        }

        public IActionResult ViewAppointmentDetail()
        {
            return View();  // This will render /Views/Staff/ServiceAppointList.cshtml
        }

        public IActionResult ViewPatient()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult ViewPatientDetail()
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
