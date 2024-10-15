using System;
using System.Collections.Generic;

namespace test2.Data;

public class Patient
{
    public string PId { get; set; }
    public string Name { get; set; }
    public string PatientImg { get; set; }
    public string Phone { get; set; }
    public string Gender { get; set; }
    public DateTime DOB { get; set; }

    // Navigation property to Account
    public virtual Account Account { get; set; }

    // Navigation property to related Feedback and HealthRecords
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; }
    public virtual ICollection<HealthRecord> HealthRecords { get; set; }
}

