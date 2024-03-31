using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string RecipientId { get; set; } = null!;

    public string PatientAccount { get; set; } = null!;

    [Required]
    public int ReleaseInformationId { get; set; }
    
    [Required]
    public int PatientAccountId { get; set; }

    public string? ReferringProvider { get; set; }

    public string? AuthorizationNumber { get; set; }

    [Required]
    public int Sequence { get; set; }

    [Required]
    public int DiagnosisId { get; set; }

    [Required]
    public bool Enabled { get; set; }

    [Required]
    public int WeeklyApprovedRBT { get; set; }

    [Required]
    public int WeeklyApprovedAnalyst { get; set; }

    [ForeignKey("ReleaseInformationId")]
    public ReleaseInformation? ReleaseInformation { get; set; }

    [ForeignKey("DiagnosisId")]
    public Diagnosis? Diagnosis { get; set; }
    
    [ForeignKey("PatientAccountId")]
    public PatientAccount PatientAccounts { get; set; } = null!;

    public virtual List<Agreement>? Agreements { get; set; }
}
