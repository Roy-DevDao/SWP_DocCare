using System.Linq;
using System.Security.Claims;
using test2.Data;
using test2.Models;

namespace test2.DAO
{
    public class UserDAO
    {
        private readonly DocCareContext _context = new DocCareContext();

        public UserDAO()
        {
        }

        public PatientProfileViewModel GetLoggedInUser(ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                return null;
            }

            // Lấy thông tin tài khoản
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);
            if (account == null)
            {
                return null;
            }

            // Lấy thông tin bệnh nhân tương ứng
            var patient = _context.Patients.FirstOrDefault(p => p.Pid == account.Id); // Sử dụng Pid để tham chiếu đến Account.Id

            // Trả về một đối tượng ViewModel
            return new PatientProfileViewModel
            {
                Id = account.Id,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role,
                Status = account.Status,
                Name = patient?.Name,
                Phone = patient?.Phone,
                Gender = patient?.Gender,
                Dob = patient?.Dob,
                PatientImg = patient?.PatientImg
            };
        }

    }
}
