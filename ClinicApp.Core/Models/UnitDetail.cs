using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class UnitDetail
{
    [Key]
    public int Id { get; set; }

    public string? Modifiers { get; set; }

    [Required]
    public int PlaceOfServiceId { get; set; }

    [Required]
    public DateTime? DateOfService { get; set; }

    [Required]
    public int Unit { get; set; }

    [Required]
    public int ServiceLogId { get; set; }

    [Required]
    public int ProcedureId { get; set; }

    [ForeignKey("PlaceOfServiceId")]
    public PlaceOfService PlaceOfService { get; set; } = null!;

    [ForeignKey("ServiceLogId")]
    public ServiceLog ServiceLog { get; set; } = null!;

    [ForeignKey("ProcedureId")]
    public Procedure Procedure { get; set; } = null!;
}
