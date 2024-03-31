using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Core.Models;

public class Agreement
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public int PayrollId { get; set; }

    [Required]
    public double RateEmployees { get; set; }

    [ForeignKey("ClientId")]
    public Client Client { get; set; } = null!;

    [ForeignKey("CompanyId")]
    public Company Company { get; set; } = null!;

    [ForeignKey("PayrollId")]
    public Payroll Payroll { get; set; } = null!;
}
