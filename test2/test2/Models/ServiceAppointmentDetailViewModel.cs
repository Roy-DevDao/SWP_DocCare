namespace test2.Models
{
    public class ServiceAppointmentDetailViewModel
    {
        public string AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string PatientGender { get; set; }
        public DateOnly? PatientDOB { get; set; }
        public string PatientImage { get; set; }
        public string SpecialtyName { get; set; }
        public string SpecialtyImage { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Status { get; set; }
        public double Fee { get; set; }
        public string SupportingStaff { get; set; }
        public string ConsultationInfo { get; set; }
    }
}

