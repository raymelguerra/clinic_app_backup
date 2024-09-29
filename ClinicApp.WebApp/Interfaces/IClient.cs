using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IClient
    {
        public Task<IEnumerable<Client>> GetClientAsync(string filter);
        public Task<Client?> GetClientAsync(int id);
        public Task<bool> PutClientAsync(int id, Client client);
        public Task<bool> PostClientAsync(Client client);
        public Task<bool> DeleteClientAsync(int id);
        public Task<IEnumerable<Client>> GetClientsByContractorAndInsurance(int contractorId, int insuranceId);
    }
}
