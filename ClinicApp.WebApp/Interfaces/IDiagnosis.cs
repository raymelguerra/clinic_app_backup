using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IDiagnosis
    {
        public Task<IEnumerable<Diagnosis>> GetDiagnosisAsync(string filter);
        public Task<Diagnosis?> GetDiagnosisAsync(int id);
        public Task<bool> PutDiagnosisAsync(int id, Diagnosis Diagnosis);
        public Task<bool> PostDiagnosisAsync(Diagnosis Diagnosis);
        public Task<bool> DeleteDiagnosisAsync(int id);
    }
}
