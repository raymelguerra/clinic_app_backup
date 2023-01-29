using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services
{
    public class DiagnosesService : IDiagnoses
    {
        private readonly ClinicbdMigrationContext _context;
        public DiagnosesService(ClinicbdMigrationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Diagnosis>> Get()
        {
            return await _context.Diagnoses.ToListAsync();
        }

        public async Task<Diagnosis?> Get(int id)
        {
            var company = await _context.Diagnoses.FindAsync(id);

            if (company == null)
            {
                return null;
            }

            return company;
        }
    }
}
