namespace ClinicApp.Core.Models;

public class ContractorServiceLog
{
    public int Id { get; set; }
    public int ServiceLogId { get; set; }
    public string? Signature { get; set; }
    public DateTime? SignatureDate { get; set; }
    public virtual ServiceLog ServiceLog { get; set; } = null!;
}
