using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test2.Context;
using test2.Data;
using test2.Models;

namespace test2.Controllers
{
    public class StaffController : Controller
    {
        private readonly DocCareContext dc;

        private readonly ILogger<StaffController> _logger;

        public StaffController(ILogger<StaffController> logger, DocCareContext dc)
        {
            _logger = logger;
            this.dc = dc;

        }

        public IActionResult AppoitmentList()
        {
            var appointments = dc.Orders.Include(o => o.PidNavigation).Include(o => o.Option)       
                .ThenInclude(opt => opt.DidNavigation) 
            .Select(o => new AppointmentViewModel
            {
                AppointmentId = o.Oid,
                PatientName = o.PidNavigation.Name,
                DoctorName = o.Option.DidNavigation.Name,
                AppointmentDate = o.Option.DateExam.HasValue ? o.Option.DateExam.Value : DateTime.MinValue,
                Status = o.Status
            })
            .ToList();

            return View(appointments);
        }

        public IActionResult AppointmentDetail()
        {
            return View();  // This will render /Views/Staff/AppointmentDetail.cshtml
        }

        public IActionResult ServiceAppointList()
        {

            var appointments = dc.Orders.Include(o => o.PidNavigation)
            .Include(o => o.Option)
         .ThenInclude(opt => opt.DidNavigation)
         .ThenInclude(d => d.Specialty) // Include Specialty
         .Select(o => new ServiceAppointmentModel
         {
             AppointmentId = o.Oid,
             PatientName = o.PidNavigation.Name,
             DoctorName = o.Option.DidNavigation.Name,
             SpecialtyName = o.Option.DidNavigation.Specialty.SpecialtyName,
             AppointmentDate = o.Option.DateExam.HasValue ? o.Option.DateExam.Value : DateTime.MinValue,
             Status = o.Status
         })
         .ToList();

            return View(appointments);
        }

        public IActionResult ServiceAppointDetail()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult Schedule()
        {
            return View();  // This will render /Views/Staff/ServiceAppointDetail.cshtml
        }

        public IActionResult ContactList(string status = "all")
        {
            var contactsQuery = dc.Contacts.AsQueryable();

            // Filter contacts based on status
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                contactsQuery = contactsQuery.Where(c => c.Status == status);
            }

            var contacts = contactsQuery
                .Select(c => new ContactViewModel
                {
                    ContactId = c.ContactId,
                    FullName = c.Name,
                    Email = c.Email,
                    Description = c.Description,
                    Status = c.Status
                })
                .ToList();

            return View(contacts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
