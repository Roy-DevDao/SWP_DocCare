namespace test2.Models
{
    public class Patient
    {
        public Account Account { get; set; }
        public Appointment Appointment { get; set; }
        public int PatientId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }

        public Patient()
        {
        }

        public Patient(int patientId, string username)
        {
            this.PatientId = patientId;
            this.Username = username;
        }

        public Patient(Account account, int patientId, string username, int roleId, bool status, string address, DateTime dob)
        {
            this.Account = account;
            this.PatientId = patientId;
            this.Username = username;
            this.RoleId = roleId;
            this.Status = status;
            this.Address = address;
            this.DOB = dob;
        }

        public Patient(Account account, int patientId, DateTime dob, string address, bool status)
        {
            this.Account = account;
            this.PatientId = patientId;
            this.DOB = dob;
            this.Address = address;
            this.Status = status;
        }

        public Patient(Account account, int patientId, DateTime dob, bool status)
        {
            this.Account = account;
            this.PatientId = patientId;
            this.DOB = dob;
            this.Status = status;
        }

        public Patient(Account account, DateTime dob, int patientId, Appointment appointment)
        {
            this.Account = account;
            this.DOB = dob;
            this.PatientId = patientId;
            this.Appointment = appointment;
        }

        public Patient(Account account)
        {
            this.Account = account;
        }

        public Patient(Account account, DateTime dob)
        {
            this.Account = account;
            this.DOB = dob;
        }
    }
}

