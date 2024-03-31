using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Dto;

[Keyless]
public class ServiceLogWithoutPatientAccount
{
    public string? Client { get; set; }
    public int? ClientId { get; set; }
    public string? Contractor { get; set; }
    public int? ContractorId { get; set; }
    public DateTime DateOfService { get; set; }
    public string? Procedure { get; set; }
    public int? SubProcedureId { get; set; }
}
