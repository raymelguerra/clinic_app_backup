using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;

namespace ClinicApp.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientAccountsController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public PatientAccountsController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/PatientAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientAccount>>> GetPatientAccounts()
        {
            return await _context.PatientAccounts.ToListAsync();
        }

        // GET: api/PatientAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientAccount>> GetPatientAccount(int id)
        {
            var patientAccount = await _context.PatientAccounts.FindAsync(id);

            if (patientAccount == null)
            {
                return NotFound();
            }

            return patientAccount;
        }

        // PUT: api/PatientAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatientAccount(int id, PatientAccount patientAccount)
        {
            if (id != patientAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(patientAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientAccountExists(id))
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

        // POST: api/PatientAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PatientAccount>> PostPatientAccount(PatientAccount patientAccount)
        {
            _context.PatientAccounts.Add(patientAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatientAccount", new { id = patientAccount.Id }, patientAccount);
        }

        // DELETE: api/PatientAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatientAccount(int id)
        {
            var patientAccount = await _context.PatientAccounts.FindAsync(id);
            if (patientAccount == null)
            {
                return NotFound();
            }

            _context.PatientAccounts.Remove(patientAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientAccountExists(int id)
        {
            return _context.PatientAccounts.Any(e => e.Id == id);
        }
    }
}
