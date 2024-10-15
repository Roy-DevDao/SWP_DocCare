using System;
using System.Collections.Generic;

namespace test2.Data;

public class Specialty
{
    public string SpecialtyId { get; set; }
    public string SpecialtyName { get; set; }
    public string SpecialtyImg { get; set; }
    public string ShortDescription { get; set; }

    // Navigation property for related Doctors and DetailSpecialties
    public virtual ICollection<Doctor> Doctors { get; set; }
    public virtual ICollection<DetailSpecialty> DetailSpecialties { get; set; }
}

