using test2.Data;
using test2.Models;

namespace test2.DAO
{
    public class PatientDao
    {
        private readonly DocCareContext _context;

        public PatientDao(DocCareContext context)
        {
            _context = context;
        }

        public List<HealthRecordViewModel> GetHealthRecordsByDoctorId(string doctorId)
        {
            // Kiểm tra đầu vào doctorId để ngăn chặn các cuộc gọi cơ sở dữ liệu không cần thiết
            if (string.IsNullOrEmpty(doctorId))
            {
                return new List<HealthRecordViewModel>();
            }

            // Lấy danh sách hồ sơ sức khỏe cho bác sĩ
            return _context.HealthRecords
                .Where(hr => hr.Did == doctorId)  // Lọc theo bác sĩ
                .Select(hr => new HealthRecordViewModel
                {
                    RecordId = hr.RecordId,
                    PatientName = hr.Patient.Name,
                    DoctorName = hr.Doctor.Name,
                    Date = hr.Date,
                    Symptoms = hr.Symptoms,
                    Treatment = hr.Treatment
                })
                .ToList();
        }
    }

}
