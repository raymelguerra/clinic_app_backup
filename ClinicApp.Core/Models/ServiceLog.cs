using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class ServiceLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PeriodId { get; set; }

    [Required]
    public int ContractorId { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int InsuranceId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? BilledDate { get; set; }

    public string Biller { get; set; } = null!;

    public string Pending { get; set; } = null!;

    [ForeignKey("PeriodId")]
    public Period Period { get; set; } = null!;

    [ForeignKey("ContractorId")]
    public Contractor Contractor { get; set; } = null!;

    [ForeignKey("ClientId")]
    public Client Client { get; set; } = null!;

    [ForeignKey("InsuranceId")]
    public Insurance Insurance { get; set; } = null!;

    public virtual List<UnitDetail>? UnitDetails { get; set; }
}
