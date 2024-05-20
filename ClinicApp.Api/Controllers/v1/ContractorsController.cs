using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace ClinicApp.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class ContractorsController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public ContractorsController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/Contractors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contractor>>> GetContractors()
        {
            return await _context.Contractors.ToListAsync();
        }

        // GET: api/Contractors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contractor>> GetContractor(int id)
        {
            var contractor = await _context.Contractors
                .Where(c => c.Id == id)
                .Include(c => c.Payrolls)
                    .ThenInclude(p => p.ContractorType)
                .Include(c => c.Payrolls)
                    .ThenInclude(p => p.InsuranceProcedure)
                        .ThenInclude(ip => ip.Insurance)
                .Include(c => c.Payrolls)
                    .ThenInclude(p => p.InsuranceProcedure)
                        .ThenInclude(ip => ip.Procedure)
                .Include(c => c.Payrolls)
                    .ThenInclude(p => p.Company)
                .FirstOrDefaultAsync();

            if (contractor == null)
            {
                return NotFound();
            }

            return contractor;
        }

        // PUT: api/Contractors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContractor(int id, Contractor contractor)
        {
            if (id != contractor.Id)
            {
                return BadRequest();
            }

            var existingContractor = await _context.Contractors.Include(c => c.Payrolls).FirstOrDefaultAsync(c => c.Id == id);

            if (existingContractor == null)
            {
                return NotFound();
            }

            // Actualizar el Contractor con los valores del nuevo contractor
            _context.Entry(existingContractor).CurrentValues.SetValues(contractor);

            foreach (var payroll in contractor.Payrolls)
            {
                if (payroll.Id == 0)
                {
                    // Es un nuevo Payroll, agregarlo al Contractor existente
                    existingContractor.Payrolls.Add(payroll);
                }
                else
                {
                    var existingPayroll = existingContractor.Payrolls.FirstOrDefault(p => p.Id == payroll.Id);
                    if (existingPayroll != null)
                    {
                        // Actualizar el Payroll existente
                        _context.Entry(existingPayroll).CurrentValues.SetValues(payroll);
                    }
                }
            }

            // Identificar y eliminar los Payrolls que fueron eliminados del Contractor
            foreach (var existingPayroll in existingContractor.Payrolls.ToList())
            {
                if (!contractor.Payrolls.Any(p => p.Id == existingPayroll.Id))
                {
                    // El Payroll existente no está presente en la lista enviada, por lo tanto, eliminarlo
                    _context.Payrolls.Remove(existingPayroll);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contractors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contractor>> PostContractor(Contractor contractor)
        {
            _context.Contractors.Add(contractor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContractor", new { id = contractor.Id }, contractor);
        }

        // DELETE: api/Contractors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContractor(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return NotFound();
            }

            _context.Contractors.Remove(contractor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContractorExists(int id)
        {
            return _context.Contractors.Any(e => e.Id == id);
        }
    }
}
