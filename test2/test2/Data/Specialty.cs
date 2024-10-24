using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace test2.Data;

public partial class Specialty
{
    [Required(ErrorMessage = "Specialty ID is required.")]
    [RegularExpression(@"^[A-Za-z]\d+$", ErrorMessage = "Specialty ID must start with a letter followed by digits.")]
    public string SpecialtyId { get; set; } = null!;

    [Required(ErrorMessage = "Specialty Name is required.")]
    [MaxLength(30, ErrorMessage = "Specialty Name cannot be longer than 30 characters.")]
    [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Specialty Name must contain only letters.")]
    public string? SpecialtyName { get; set; }

    public string? SpecialtyImg { get; set; }

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
