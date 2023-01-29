using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services
{
    public class SubProcedureService : ISubProcedure
    {
        private readonly ClinicbdMigrationContext _context;
        public SubProcedureService(ClinicbdMigrationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubProcedure>> Get()
        {
            return await _context.SubProcedures.ToListAsync();
        }

        public async Task<SubProcedure?> Get(int id)
        {
            var company = await _context.SubProcedures.FindAsync(id);

            if (company == null)
            {
                return null;
            }

            return company;
        }

        public async Task<IEnumerable<SubProcedure>> GetSubProceduresByAgreement(int clientId, int contractorId)
        {
            var query = await (from a in _context.Agreements
                               join p in _context.Payrolls on a.PayrollId equals p.Id
                               join c in _context.Clients on a.ClientId equals c.Id
                               where p.ContractorId == contractorId && c.Id == clientId
                               select new { procedureId = p.ProcedureId }).Distinct().FirstOrDefaultAsync();
            return await _context.SubProcedures.Include(x => x.Procedure).Where(x => x.ProcedureId == query!.procedureId).ToListAsync<SubProcedure>();
        }
    }
}
