using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces;

public interface IInsurance
{
    public Task<IEnumerable<Insurance>> GetInsuranceAsync(string filter);
    public Task<Insurance?> GetInsuranceAsync(int id);
    public Task<bool> PutInsuranceAsync(int id, Insurance insurance);
    public Task<bool> PostInsuranceAsync(Insurance insurance);
    public Task<bool> DeleteInsuranceAsync(int id);
}
