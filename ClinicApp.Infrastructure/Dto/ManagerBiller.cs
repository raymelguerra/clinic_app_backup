
namespace ClinicApp.Infrastructure.Dto;

public class ManagerBiller
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string ContractorName { get; set; }
    public DateTime CreationDate { get; set; }
    public string Biller { get; set; }
    public DateTime? BilledDate { get; set; }
}

