namespace test2.Models
{
    public class Doctor
    {
        public Staff staff { get; set; }
        public int DoctorId { get; set; }
        public int RoleId { get; set; }
        public string DoctorName { get; set; }
        public Account Account { get; set; }
        public bool Gender { get; set; }
        public DateTime DOB { get; set; }
        public int Phone { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string Img { get; set; }
        public RateStar RateStar { get; set; }
        public double Fee { get; set; }
        public string Position { get; set; }

        public Doctor()
        {
        }

        public Doctor(Staff setting, int doctorId, int roleId, string doctorName, Account account, bool gender, DateTime dob, int phone, string description, bool status, string img, double fee, string position)
        {
            this.staff = setting;
            this.DoctorId = doctorId;
            this.RoleId = roleId;
            this.DoctorName = doctorName;
            this.Account = account;
            this.Gender = gender;
            this.DOB = dob;
            this.Phone = phone;
            this.Description = description;
            this.Status = status;
            this.Img = img;
            this.Fee = fee;
            this.Position = position;
        }

        public Doctor(Staff setting, int doctorId, int roleId, string doctorName, Account account, bool gender, DateTime dob, int phone, string description, bool status, string img, RateStar rateStar, double fee, string position)
        {
            this.staff = setting;
            this.DoctorId = doctorId;
            this.RoleId = roleId;
            this.DoctorName = doctorName;
            this.Account = account;
            this.Gender = gender;
            this.DOB = dob;
            this.Phone = phone;
            this.Description = description;
            this.Status = status;
            this.Img = img;
            this.RateStar = rateStar;
            this.Fee = fee;
            this.Position = position;
        }

        public Doctor(string img, string doctorName, int phone, bool gender, string description)
        {
            this.DoctorName = doctorName;
            this.Gender = gender;
            this.Phone = phone;
            this.Description = description;
            this.Img = img;
        }

        public Doctor(Staff setting, int doctorId, string doctorName, bool gender, bool status)
        {
            this.staff = setting;
            this.DoctorId = doctorId;
            this.DoctorName = doctorName;
            this.Gender = gender;
            this.Status = status;
        }

        public Doctor(string doctorName)
        {
            this.DoctorName = doctorName;
        }

        public Doctor(int doctorId, string doctorName)
        {
            this.DoctorId = doctorId;
            this.DoctorName = doctorName;
        }

        public Doctor(int doctorId, string img, string doctorName)
        {
            this.DoctorId = doctorId;
            this.DoctorName = doctorName;
            this.Img = img;
        }
    }

}
