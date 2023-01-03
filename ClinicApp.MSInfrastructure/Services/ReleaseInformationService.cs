using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services
{
    public class ReleaseInformationService : IReleaseInformation
    {
        private readonly ClinicbdMigrationContext _context;
        public ReleaseInformationService(ClinicbdMigrationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReleaseInformation>> Get()
        {
            return await _context.ReleaseInformations.ToListAsync();
        }

        public async Task<ReleaseInformation?> Get(int id)
        {
            var company = await _context.ReleaseInformations.FindAsync(id);

            if (company == null)
            {
                return null;
            }

            return company;
        }
    }
}
