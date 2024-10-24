namespace test2.Models
{
    public class PatientProfileViewModel
    {
        public string PId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int? Role { get; set; }
        public bool? Status { get; set; }

        // Thuộc tính từ lớp Patient
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateOnly? Dob { get; set; }
        public string? PatientImg { get; set; }
    }
}
