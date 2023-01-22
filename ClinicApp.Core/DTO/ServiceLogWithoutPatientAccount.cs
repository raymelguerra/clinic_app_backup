using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.DTO;

[Keyless]
public class ServiceLogWithoutPatientAccount
{
    public string? Client { get; set; }
    public string? ClientId { get; set; }
    public string? Contractor { get; set; }
    public string? ContractorId { get; set; }
    public DateTime DateOfService { get; set; }
    public string? Procedure { get; set; }
    public string? SubProcedureId { get; set; }
}
