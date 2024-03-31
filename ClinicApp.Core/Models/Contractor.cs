using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class Contractor
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? RenderingProvider { get; set; }

    [Required]
    public bool Enabled { get; set; }

    public string? Extra { get; set; }

    public virtual List<Payroll>? Payrolls { get; set; }
}
