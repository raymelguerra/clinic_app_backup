using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services
{
    public class CompaniesService : ICompanies
    {
        private readonly ClinicbdMigrationContext _context;
        public CompaniesService(ClinicbdMigrationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> Get()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> Get(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return null;
            }

            return company;
        }
    }
}
