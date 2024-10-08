using System;
using System.Collections.Generic;

namespace Admin.Models;

public partial class Specialty
{
    public string SpecialtyId { get; set; } = null!;

    public string? SpecialtyName { get; set; }

    public string? SpecialtyImg { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
