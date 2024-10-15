using System;
using System.Collections.Generic;

namespace test2.Data;

public class Schedule
{
    public string ScheduleId { get; set; }
    public DateTime DateWork { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }

    // Navigation property for related Options
    public virtual ICollection<Option> Options { get; set; }
}

