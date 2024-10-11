using test2.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace test2.DAO
{
    public class PatientDao
    {
        private readonly DocCareContext dc;

        public PatientDao(DocCareContext context)
        {
            dc = context;
        }

        // Lấy tất cả bệnh nhân của bác sĩ theo Did
        public List<Patient> GetPatientsByDoctorId(string did)
        {
            return dc.Patients
                .Include(p => p.Orders) // Bao gồm Orders của bệnh nhân
                .ThenInclude(o => o.Option) // Bao gồm Option của từng Order
                .Include(p => p.PidNavigation) // Bao gồm thông tin tài khoản của bệnh nhân
                .Where(p => p.Orders.Any(o => o.Option != null && o.Option.Did == did))
                .ToList();
        }

        // Lấy chi tiết một bệnh nhân theo ID
        public Patient? GetPatientById(string pid)
        {
            return dc.Patients
                .Include(p => p.Orders) // Bao gồm Orders của bệnh nhân
                .ThenInclude(o => o.Option) // Bao gồm Option của từng Order
                .Include(p => p.PidNavigation) // Bao gồm thông tin tài khoản của bệnh nhân
                .FirstOrDefault(p => p.Pid == pid);
        }
    }
}
