using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;

namespace ClinicApp.MSDashboard.Interfaces;

public interface IDashboard
{
    public Task<IEnumerable<ProfitHistory>> GetProfit(int company_id);
    public Task<ServicesLogStatus> GetServicesLgStatus(int company_id, int period_id);
    public Task<IEnumerable<ServiceLogWithoutPatientAccount>> GetServiceLogWithoutPatientAccount(int company_id, int period_id);
    public Task<GeneralData> GetGeneralData(int company_id, int period_id);
    public Task<IEnumerable<Company>> GetCompanies();
    public Task<IEnumerable<Period>> GetPeriods();
}
