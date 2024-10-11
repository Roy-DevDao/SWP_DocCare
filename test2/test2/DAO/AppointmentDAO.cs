using System.Collections.Generic;
using System.Linq;
using test2.Data;

namespace test2.DAO
{
    public class AppointmentDAO
    {
        private readonly DocCareContext _context;

        public AppointmentDAO(DocCareContext context)
        {
            _context = context;
        }

        public List<Order> GetAppointmentsForFirstDoctor()
        {
            // Fetch the first doctor by their ID and retrieve their appointments
            var firstDoctorId = _context.Doctors.FirstOrDefault()?.Did;

            if (firstDoctorId == null)
            {
                return new List<Order>();
            }

            // Get appointments by joining related tables
            var appointments = _context.Orders
                .Where(o => o.Option.Did == firstDoctorId)
                .Select(o => new Order
                {
                    Oid = o.Oid,
                    Pid = o.Pid,
                    OptionId = o.OptionId,
                    Status = o.Status,
                    DateOrder = o.DateOrder,
                    Symptom = o.Symptom,
                    Option = o.Option,
                    PidNavigation = o.PidNavigation
                })
                .ToList();

            return appointments;


        }

        // Lấy chi tiết cuộc hẹn theo Order Id (Oid)
        public Order GetAppointmentDetailById(string oid)
        {
            var appointment = _context.Orders
                .Where(o => o.Oid == oid)
                .Select(o => new Order
                {
                    Oid = o.Oid,
                    PidNavigation = o.PidNavigation,
                    Option = o.Option,
                    DateOrder = o.DateOrder,
                    Status = o.Status,
                    Symptom = o.Symptom
                })
                .FirstOrDefault();

            // Nếu không tìm thấy cuộc hẹn, trả về cuộc hẹn đầu tiên trong danh sách
            if (appointment == null)
            {
                return _context.Orders
                    .Select(o => new Order
                    {
                        Oid = o.Oid,
                        PidNavigation = o.PidNavigation,
                        Option = o.Option,
                        DateOrder = o.DateOrder,
                        Status = o.Status,
                        Symptom = o.Symptom
                    })
                    .FirstOrDefault();
            }

            return appointment;
        }

    }
}
