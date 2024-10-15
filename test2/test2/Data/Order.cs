using System;
using System.Collections.Generic;

namespace test2.Data;

public class Order
{
    public string OId { get; set; }
    public string PId { get; set; }
    public string OptionId { get; set; }
    public string Status { get; set; }
    public DateTime DateOrder { get; set; }
    public string Symptom { get; set; }

    // Navigation property to Patient and Option
    public virtual Patient Patient { get; set; }
    public virtual Option Option { get; set; }

    public virtual ICollection<HealthRecord> HealthRecords { get; set; }
}

