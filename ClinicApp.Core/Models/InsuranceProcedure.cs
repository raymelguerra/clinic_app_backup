using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class InsuranceProcedure
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InsuranceId { get; set; }

    [Required]
    public int ProcedureId { get; set; }

    [Required]
    public double Rate { get; set; }

    [ForeignKey("InsuranceId")]
    public Insurance Insurance { get; set; } = null!;

    [ForeignKey("ProcedureId")]
    public Procedure Procedure { get; set; } = null!;
}
