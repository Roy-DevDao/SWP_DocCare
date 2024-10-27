namespace test2.Models.DoctorModel
{
    public class BaseViewModel
    {
        public string? DoctorImg { get; set; }
        public string DId { get; set; } = null!;
        public string? Name { get; set; }

        public DoctorProfileViewModel doctorProfile { get; set; }

        public AppointmentViewModel appointmentlist { get; set; }
    }
}
