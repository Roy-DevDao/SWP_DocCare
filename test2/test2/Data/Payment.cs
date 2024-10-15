using System;
using System.Collections.Generic;

namespace test2.Data;

public class Payment
{
    public string PayId { get; set; }
    public string OId { get; set; }
    public string Method { get; set; }
    public string PayImg { get; set; }
    public DateTime DatePay { get; set; }

    // Navigation property to Order
    public virtual Order Order { get; set; }
}

