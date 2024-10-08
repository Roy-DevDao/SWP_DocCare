using System;
using System.Collections.Generic;

namespace Admin.Models;

public partial class Schedule
{
    public string ScheduleId { get; set; } = null!;

    public string? Did { get; set; }

    public DateOnly? DateWork { get; set; }

    public TimeOnly? TimeWork { get; set; }

    public string? Status { get; set; }

    public virtual Doctor? DidNavigation { get; set; }
}
