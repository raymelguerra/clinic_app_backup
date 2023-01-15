using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSClient.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.MSClient.Interfaces;
public interface IClient
{
    public Task<PagedResponse<IEnumerable<Client>>> GetClient([FromQuery] PaginationFilter filter, string route);
    public Task<Client?> GetClient(int id);
    public Task<IEnumerable<Client>> GetClientWithoutDetails();
    public Task<PagedResponse<IEnumerable<Client>>> GetClientByName([FromQuery] PaginationFilter filter, string name, string route);
    public Task<IEnumerable<Client>> GetClientsByContractor(int id);
    public Task<object?> PutClient(int id, Client client);
    public Task<Client?> PostClient(Client client);
    public Task<object?> DeleteClient(int id);
    public bool ClientExists(int id);

    public Task<IEnumerable<Agreement>> GetAgreement();
    public Task<IEnumerable<Agreement>?> GetAgreementByContractor(int id);
    public Task<Agreement?> GetAgreement(int id);
    public Task<object?> PutAgreement(int id, Agreement agreement);
    public Task<Agreement?> PostAgreement(Agreement agreement);
    public Task<object?> DeleteAgreement(int id);
    public bool AgreementExists(int id);

    public Task<IEnumerable<PatientAccount>> GetBilling();
    public Task<PatientAccount?> GetPatientAccount(int id);
    public Task<IEnumerable<PatientAccount>> GetBilling(int idclient);
    public Task<object?> PutPatientAccount(int id, PatientAccount patient);
    public Task<PatientAccount?> PostPatientAccount(PatientAccount patient);
    public Task<object?> DeletePatientAccount(int id);
    public bool PatientAccountExists(int id);
}
