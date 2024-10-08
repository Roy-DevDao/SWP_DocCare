using System;
using System.Collections.Generic;

namespace Admin.Models;

public partial class Option
{
    public string OptionId { get; set; } = null!;

    public string? Did { get; set; }

    public DateTime? DateExam { get; set; }

    public virtual Doctor? DidNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
