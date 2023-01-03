using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
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
    }
}
