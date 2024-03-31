using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IDiagnosis
    {
        public Task<IEnumerable<Diagnosis>> GetDiagnosisAsync(string filter);
        public Task<Diagnosis?> GetDiagnosisAsync(int id);
    }
}
