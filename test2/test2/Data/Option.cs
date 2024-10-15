using System;
using System.Collections.Generic;

namespace test2.Data;

public class Option
{
    public string OptionId { get; set; }
    public string DId { get; set; }
    public string ScheduleId { get; set; }
    public DateTime DateExam { get; set; }
    public string Status { get; set; }

    // Navigation property to Doctor and Schedule
    public virtual Doctor Doctor { get; set; }
    public virtual Schedule Schedule { get; set; }

    public virtual ICollection<HealthRecord> HealthRecords { get; set; }
}

