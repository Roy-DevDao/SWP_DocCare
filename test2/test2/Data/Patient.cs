//using System;
//using System.Collections.Generic;

//namespace test2.Data;

//public partial class Patient
//{
//    public string Pid { get; set; } = null!;

//    public string? Name { get; set; }

//    public string? PatientImg { get; set; }

//    public string? Phone { get; set; }

//    public string? Gender { get; set; }

//    public DateOnly? Dob { get; set; }

//    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

//    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

//    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

//    public virtual Account PidNavigation { get; set; } = null!;
//}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test2.Data
{
    public partial class Patient
    {
        public string Pid { get; set; } = null!;

        public string? Name { get; set; }

        public string? PatientImg { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date)]
        [CheckAge(18, ErrorMessage = "Bệnh nhân phải trên 18 tuổi.")]
        public DateOnly? Dob { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        [ValidateNever]

        public virtual Account PidNavigation { get; set; } = null!;
    }

    public class CheckAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public CheckAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly dob)
            {
                // Convert DateOnly to DateTime for comparison
                DateTime dobDateTime = dob.ToDateTime(TimeOnly.MinValue);
                int age = DateTime.Now.Year - dobDateTime.Year;

                if (dobDateTime.AddYears(age) >= DateTime.Now)
                    age--;

                if (age < _minimumAge)
                    return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}
