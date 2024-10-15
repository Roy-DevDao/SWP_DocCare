using System;
using System.Collections.Generic;

namespace test2.Data;

public class Feedback
{
    public string FeedbackId { get; set; }
    public string DId { get; set; }
    public string PId { get; set; }
    public string Name { get; set; }
    public DateTime DateCmt { get; set; }
    public int Star { get; set; }
    public string Description { get; set; }

    // Navigation property to Doctor and Patient
    public virtual Doctor Doctor { get; set; }
    public virtual Patient Patient { get; set; }
}

