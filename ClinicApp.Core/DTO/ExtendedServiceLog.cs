using ClinicApp.Core.Models;

namespace ClinicApp.Core.DTO;

public class ExtendedServiceLog
{
    public ServiceLog? serviceLog { get; set; }
    public Agreement? agreement { get; set; }
    public Client? client { get; set; }
    public Diagnosis? diagnosis { get; set; }
    public Period? period { get; set; }
    public Payroll? payroll { get; set; }
    public Procedure? procedure { get; set; }
    public Contractor? contractor { get; set; }
    public ContractorType? contractorType { get; set; }
}