using System;
using System.Collections.Generic;

namespace test2.Data;

public class HealthRecord
{
    public string RecordId { get; set; }
    public string PId { get; set; }
    public string DId { get; set; }
    public string OId { get; set; }
    public string Diagnosis { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public DateTime DateExam { get; set; }

    // Navigation property to Patient, Doctor and Order
    public virtual Patient Patient { get; set; }
    public virtual Doctor Doctor { get; set; }
    public virtual Order Order { get; set; }
}

