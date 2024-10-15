using System;
using System.Collections.Generic;

namespace test2.Data;

public class Doctor
{
    public string DId { get; set; }
    public string Name { get; set; }
    public string DoctorImg { get; set; }
    public string Position { get; set; }
    public string Phone { get; set; }
    public string Gender { get; set; }
    public DateTime DOB { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string SpecialtyId { get; set; }

    // Navigation properties for related Account and Specialty
    public virtual Account Account { get; set; }
    public virtual Specialty Specialty { get; set; }

    // Navigation property for related Feedback, HealthRecords, and DetailDoctor
    public virtual ICollection<Feedback> Feedbacks { get; set; }
    public virtual ICollection<HealthRecord> HealthRecords { get; set; }
    public virtual ICollection<DetailDoctor> DetailDoctors { get; set; }
}


