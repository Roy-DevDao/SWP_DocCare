using System;
using System.Collections.Generic;

namespace test2.Data;

public class Account
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public bool Status { get; set; }

    // Navigation property for related patients and doctors
    public virtual ICollection<Patient> Patients { get; set; }
    public virtual ICollection<Doctor> Doctors { get; set; }
}


