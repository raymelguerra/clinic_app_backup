using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.MSInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services
{
    public class ContractorTypeService : IContractorType
    {
        private readonly ClinicbdMigrationContext _context;
        public ContractorTypeService(ClinicbdMigrationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContractorType>> Get()
        {
            return await _context.ContractorTypes.ToListAsync();
        }

        public async Task<ContractorType?> Get(int id)
        {
            var company = await _context.ContractorTypes.FindAsync(id);

            if (company == null)
            {
                return null;
            }

            return company;
        }
    }
}
