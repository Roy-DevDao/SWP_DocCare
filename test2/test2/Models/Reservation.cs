namespace test2.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Service Service { get; set; }
        public DateTime Date { get; set; }  // Sử dụng DateTime thay cho Date và Time
        public TimeSpan Time { get; set; }
        public string Status { get; set; }
        public Account Staff { get; set; }
        public string Description { get; set; }

        public Reservation()
        {
        }

        public Reservation(int id, Patient patient, Service service, DateTime date, TimeSpan time, string status, Account staff, string description)
        {
            this.Id = id;
            this.Patient = patient;
            this.Service = service;
            this.Date = date;
            this.Time = time;
            this.Status = status;
            this.Staff = staff;
            this.Description = description;
        }

        public Reservation(int id, Patient patient, Service service, DateTime date, TimeSpan time, string status)
        {
            this.Id = id;
            this.Patient = patient;
            this.Service = service;
            this.Date = date;
            this.Time = time;
            this.Status = status;
        }
    }

}
