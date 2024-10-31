using test2.Models.StaffModel;

namespace test2.Models.DoctorModel
{
    public class BaseViewModel
    {
        public string? DoctorImg { get; set; }
        public string DId { get; set; } = null!;
        public string? Name { get; set; }

        public DoctorProfileViewModel doctorProfile { get; set; }

        public AppointmentViewModel appointmentlist { get; set; }

        public AppointmentDetailViewModel appointmentDetail { get; set; }

        public PatientViewModel patientView { get; set; }

        public PatientDetailViewModel patientDetail { get; set; }

        public FeedbackViewModel feedbackView { get; set; }
        public HealthRecordViewModel healthRecord { get; set; }

    }
}