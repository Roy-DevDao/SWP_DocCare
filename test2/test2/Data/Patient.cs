using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test2.Data;

public partial class Patient
{
    [Required(ErrorMessage = "Patient ID is required.")]
    [RegularExpression(@"^[A-Za-z]\d+$", ErrorMessage = "Patient ID must start with a letter followed by digits.")]
    public string Pid { get; set; } = null!;

    [Required(ErrorMessage = "Specialty Name is required.")]
    [MaxLength(30, ErrorMessage = "Specialty Name cannot be longer than 30 characters.")]
    [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Specialty Name must contain only letters.")]
    public string? Name { get; set; }

    public string? PatientImg { get; set; }

    [Required(ErrorMessage ="Phone number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage ="Phone number must be 10 digits.")]
    public string? Phone { get; set; }

    public string? Gender { get; set; }


    public DateOnly? Dob { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ValidateNever] // Add this attribute to skip validation for this property
    public virtual Account PidNavigation { get; set; } = null!;
}
