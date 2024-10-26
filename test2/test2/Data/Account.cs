using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test2.Data;

public partial class Account

{

    [Required(ErrorMessage = "Patient ID is required.")]
    [RegularExpression(@"^[A-Za-z]\d+$", ErrorMessage = "Account ID must start with a letter followed by digits.")]
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? Role { get; set; }

    public bool? Status { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
