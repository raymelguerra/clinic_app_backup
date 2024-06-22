using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class InsurancesController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public InsurancesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/Insurances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insurance>>> GetInsurances()
        {
            return await _context.Insurances.Include(i => i.InsuranceProcedures).ToListAsync();
        }

        // GET: api/Insurances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Insurance>> GetInsurance(int id)
        {
            var insurance = await _context.Insurances
                .Include(i => i.InsuranceProcedures)
                .ThenInclude(x => x.Procedure)
                .ThenInclude(x => x.ContractorType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (insurance == null)
            {
                return NotFound();
            }

            return insurance;
        }

        // PUT: api/Insurances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurance(int id, Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return BadRequest();
            }

            var existingInsurance = await _context.Insurances
                .Include(i => i.InsuranceProcedures)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingInsurance == null) { return NotFound(); }

            // Update insurance with the new values
            _context.Entry(existingInsurance).CurrentValues.SetValues(insurance);

            // Remove the insurance procedures that are not in the new list
            foreach (var insuranceproc in insurance.InsuranceProcedures!.ToList())
            {
                if (insuranceproc.Id == 0)
                {
                    existingInsurance.InsuranceProcedures!.Add(insuranceproc);
                }
                else
                {
                    var existingInsuranceProc = existingInsurance.InsuranceProcedures!
                        .Where(x => x.Id == insuranceproc.Id)
                        .SingleOrDefault();
                    _context.Entry(existingInsuranceProc).CurrentValues.SetValues(insuranceproc);
                }

            }

            // Add the new insurance procedures
            foreach (var existingInsProc in existingInsurance.InsuranceProcedures!)
            {
                if (!insurance.InsuranceProcedures!.Any(p => p.Id == existingInsProc.Id)) { 
                    _context.InsuranceProcedures.Remove(existingInsProc);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceExists(id))
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

        // POST: api/Insurances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Insurance>> PostInsurance(Insurance insurance)
        {
            _context.Insurances.Add(insurance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsurance", new { id = insurance.Id }, insurance);
        }

        // DELETE: api/Insurances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }

            _context.Insurances.Remove(insurance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.Id == id);
        }
    }
}
