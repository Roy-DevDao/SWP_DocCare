using System;
using System.Collections.Generic;

namespace test2.Data;

public class DetailDoctor
{
    public string DetailId { get; set; }
    public string DId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    // Navigation property to Doctor
    public virtual Doctor Doctor { get; set; }
}


