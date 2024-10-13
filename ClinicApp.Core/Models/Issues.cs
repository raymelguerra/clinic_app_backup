using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApp.Core.Models;

public class Issues
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string Issuer { get; set; } = null!;
    
    [Required]
    public int PeriodId { get; set; }
    
    public int? ContractorId { get; set; }

    public int? ClientId { get; set; }

    public bool IsActive { get; set; } = false;

    [ForeignKey("ContractorId")]
    public Contractor? Contractor { get; set; }

    [ForeignKey("ClientId")]
    public Client? Client { get; set; }

    [ForeignKey("PeriodId")]
    public Period Period { get; set; } = null!;
}
