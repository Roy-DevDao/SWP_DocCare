namespace test2.Models.DoctorModel
{
    public class HealthRecordViewModel
    {
        public string RecordId { get; set; } = Guid.NewGuid().ToString();
        public string PatientName { get; set; }
        public string AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime DateExam { get; set; }
    }

}
