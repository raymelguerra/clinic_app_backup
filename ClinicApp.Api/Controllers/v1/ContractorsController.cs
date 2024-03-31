using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;

namespace ClinicApp.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
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
            var contractor = await _context.Contractors.FindAsync(id);

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

            _context.Entry(contractor).State = EntityState.Modified;

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
