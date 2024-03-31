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
    public class ContractorTypesController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public ContractorTypesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/ContractorTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractorType>>> GetContractorTypes()
        {
            return await _context.ContractorTypes.ToListAsync();
        }

        // GET: api/ContractorTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContractorType>> GetContractorType(int id)
        {
            var contractorType = await _context.ContractorTypes.FindAsync(id);

            if (contractorType == null)
            {
                return NotFound();
            }

            return contractorType;
        }

        // PUT: api/ContractorTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContractorType(int id, ContractorType contractorType)
        {
            if (id != contractorType.Id)
            {
                return BadRequest();
            }

            _context.Entry(contractorType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractorTypeExists(id))
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

        // POST: api/ContractorTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContractorType>> PostContractorType(ContractorType contractorType)
        {
            _context.ContractorTypes.Add(contractorType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContractorType", new { id = contractorType.Id }, contractorType);
        }

        // DELETE: api/ContractorTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContractorType(int id)
        {
            var contractorType = await _context.ContractorTypes.FindAsync(id);
            if (contractorType == null)
            {
                return NotFound();
            }

            _context.ContractorTypes.Remove(contractorType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContractorTypeExists(int id)
        {
            return _context.ContractorTypes.Any(e => e.Id == id);
        }
    }
}
