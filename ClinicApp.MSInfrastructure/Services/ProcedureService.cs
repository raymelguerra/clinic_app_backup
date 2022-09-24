using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services
{
    public class ProcedureService : IProcedure
    {
        private readonly clinicbdContext _context;
        public ProcedureService(clinicbdContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Procedure>> Get()
        {
            return await _context.Procedures.ToListAsync();
        }

        public async Task<Procedure?> Get(int id)
        {
            var company = await _context.Procedures.FindAsync(id);

            if (company == null)
            {
                return null;
            }

            return company;
        }
    }
}
