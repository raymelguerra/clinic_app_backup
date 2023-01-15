using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Interfaces;

public interface ISubProcedure
{
    public Task<IEnumerable<SubProcedure>> Get();
    public Task<SubProcedure?> Get(int id);
    public Task<IEnumerable<SubProcedure>> GetSubProceduresByAgreement(int clientId, int contractorId);
}
