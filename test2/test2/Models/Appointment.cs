namespace test2.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Account Staff { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; }
        public double Fee { get; set; }
        public string Description { get; set; }

        public Appointment()
        {
        }

        public Appointment(int id, Patient patient, DateTime date, TimeSpan time, string status)
        {
            this.Id = id;
            this.Patient = patient;
            this.Date = date;
            this.Time = time;
            this.Status = status;
        }

        public Appointment(int id, Patient patient, Doctor doctor, Account staff, DateTime date, TimeSpan time, string status, double fee, string description)
        {
            this.Id = id;
            this.Patient = patient;
            this.Doctor = doctor;
            this.Staff = staff;
            this.Date = date;
            this.Time = time;
            this.Status = status;
            this.Fee = fee;
            this.Description = description;
        }

        public Appointment(int id, Patient patient, Doctor doctor, DateTime date, TimeSpan time, string status)
        {
            this.Id = id;
            this.Patient = patient;
            this.Doctor = doctor;
            this.Date = date;
            this.Time = time;
            this.Status = status;
        }

        public Appointment(DateTime date, TimeSpan time, string status)
        {
            this.Date = date;
            this.Time = time;
            this.Status = status;
        }
    }

}
