using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IPhysician
    {
        public Task<IEnumerable<Contractor>> GetPhysicianAsync(string filter);
        public Task<Contractor?> GetPhysicianAsync(int id);
        public Task<bool> PutPhysicianAsync(int id, Contractor contractor);
        public Task<bool> PostPhysicianAsync(Contractor contractor);
        public Task<bool> DeletePhysicianAsync(int id);
        public Task<IEnumerable<Contractor>> GetContractoresByProcedureAndInsurance(int procedureId, int insuranceId);
    }
}
