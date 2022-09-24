using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Interfaces
{
    public interface IDiagnoses
    {
        public Task<IEnumerable<Diagnosis>> Get();
        public Task<Diagnosis?> Get(int id);
    }
}
