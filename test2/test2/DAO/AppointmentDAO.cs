using test2.Data;

namespace test2.DAO
{
    public class AppointmentDAO
    {
        private readonly Context.DocCareContext _context;

        public AppointmentDAO(Context.DocCareContext context)
        {
            _context = context;
        }

        public List<Order> GetAppointmentsForFirstDoctor()
        {
            // Fetch the first doctor by their ID and retrieve their appointments
            var firstDoctorId = _context.Doctors.FirstOrDefault()?.DId;

            if (firstDoctorId == null)
            {
                return new List<Order>();
            }

            // Get appointments by joining related tables
            var appointments = _context.Orders
                .Where(o => o.Option.DId == firstDoctorId)
                .Select(o => new Order
                {
                    OId = o.OId,
                    PId = o.PId,
                    OptionId = o.OptionId,
                    Status = o.Status,
                    DateOrder = o.DateOrder,
                    Symptom = o.Symptom,
                    Option = o.Option,
                    Patient = o.Patient,
                })
                .ToList();

            return appointments;


        }

        // Lấy chi tiết cuộc hẹn theo Order Id (Oid)
        public Order GetAppointmentDetailById(string oid)
        {
            var appointment = _context.Orders
                .Where(o => o.OId == oid)
                .Select(o => new Order
                {
                    OId = o.OId,
                    Patient = o.Patient,
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
                        OId = o.OId,
                        Patient = o.Patient,
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
