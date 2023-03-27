namespace ClinicApp.Core.Models;
public class PatientUnitDetail
{
    public int Id { get; set; }
    public virtual int UnitDetailId { get; set; }
    
    public string? Signature { get; set; }
    public string? EntryTime { get; set; }
    public string? DepartureTime { get; set; }
    public DateTime? SignatureDate { get; set; }
    public virtual UnitDetail UnitDetail { get; set; } = null!;
}
