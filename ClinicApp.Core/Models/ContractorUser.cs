namespace ClinicApp.Core.Models;
public class ContractorUser
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int ContractorId { get; set; }

    public Contractor Contractor { get; set; } = null!;
}
