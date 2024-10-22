using System.ComponentModel.DataAnnotations;

namespace test2.Models
{
    public class ForgotPassModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
