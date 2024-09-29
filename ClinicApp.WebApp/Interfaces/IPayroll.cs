using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces;

public interface IPayroll 
{ 
    public Task<IEnumerable<Payroll>> GetPayrollAsync(string filter);
    public Task<Payroll?> GetPayrollAsync(int id);
    public Task<bool> PutPayrollAsync(int id, Payroll Payroll);
    public Task<bool> PostPayrollAsync(Payroll Payroll);
    public Task<bool> DeletePayrollAsync(int id);
    public Task<IEnumerable<Payroll>> GetPayrollsByInsuranceAsync(int insuranceId);
}
