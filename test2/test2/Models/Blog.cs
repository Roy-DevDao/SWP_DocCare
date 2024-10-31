using System;
using System.Collections.Generic;

namespace test2.Models;

public partial class Blog
{
    public string BlogId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Image { get; set; }

    public string? ShortDescription { get; set; }

    public string? Content { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateBy { get; set; }
}
