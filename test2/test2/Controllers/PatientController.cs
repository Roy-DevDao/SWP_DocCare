using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test2.Data;

namespace test2.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly DocCareContext dc;

        public PatientController(ILogger<PatientController> logger, DocCareContext db)
        {
            _logger = logger;
            dc = db;
        }
        [HttpGet]
        [Route("Patient/AppointmentHistory")]
        public IActionResult AppointmentHistory()
        {
            var list = dc.Orders
                 .Include(o => o.Option)             // Include the Option navigation property
                 .ThenInclude(op => op.DidNavigation)
                 .ThenInclude(doctor => doctor.Specialty)
                 .ToList();
            return View(list);
        }

        [HttpGet]
        [Route("Patient/AppointmentHistory/{status?}")]
        public IActionResult AppointmentHistory(string status)
        {
            var orders = dc.Orders
                           .Include(o => o.Option)
                           .ThenInclude(op => op.DidNavigation)
                           .ThenInclude(doctor => doctor.Specialty)
                           .AsQueryable();  // Allows filtering on the query later

            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status == status);  // Filter by status
            }

            var orderList = orders.ToList();  // Fetch the filtered orders

            return View(orderList);  // Pass the filtered list to the view
        }

        public IActionResult AppointmentDetail(string oid)
        {
            // Fetch the order details including all necessary related information
            var order = dc.Orders
                          .Include(o => o.Option)
                          .ThenInclude(op => op.DidNavigation)
                          .ThenInclude(doctor => doctor.Specialty)
                          .Include(o => o.PidNavigation)
                          .Include(o => o.HealthRecords)
                          .ThenInclude(hr => hr.DidNavigation)
                          .FirstOrDefault(o => o.Oid == oid);

            if (order == null)
            {
                return NotFound(); // Handle the case when the order is not found
            }

            return View(order);  // Pass the order object to the view
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