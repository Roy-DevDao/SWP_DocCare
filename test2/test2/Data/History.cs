using System;
using System.Collections.Generic;

namespace test2.Data;

public partial class History
{
    public string HistoryId { get; set; } = null!;

    public string? Pid { get; set; }

    public string? Did { get; set; }

    public string? RecordId { get; set; }

    public DateTime? DateExam { get; set; }

    public virtual Doctor? DidNavigation { get; set; }

    public virtual Patient? PidNavigation { get; set; }

    public virtual HealthRecord? Record { get; set; }
}
