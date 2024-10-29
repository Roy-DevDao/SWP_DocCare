namespace test2.Models.DoctorModel
{
    public class HealthRecordViewModel
    {
        public string RecordId { get; set; }
        public string Pid { get; set; }
        public string Did { get; set; }
        public string OrderId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public DateTime? DateExam { get; set; } // ngày khám bệnh (tức là DateWork ở bảng Option)
        public BaseViewModel basevm { get; set; } = new BaseViewModel();
    }
}
