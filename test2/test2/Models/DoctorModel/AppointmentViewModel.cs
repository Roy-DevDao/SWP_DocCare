namespace test2.Models.DoctorModel
{
    public class AppointmentViewModel
    {
        public string AppointmentId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientImage { get; set; }
        public DateTime? DateOrder { get; set; }  // Ngày hẹn
        public string? Status { get; set; }

        // Thêm thông tin về bác sĩ
        public string? DId { get; set; } // ID bác sĩ
        public string? DoctorName { get; set; } // Tên bác sĩ
        public string? DoctorImg { get; set; } // Hình ảnh bác sĩ
    }
}
