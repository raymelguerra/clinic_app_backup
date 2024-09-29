using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.OData.Query;

namespace ClinicApp.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientsController(
        InsuranceContext context
        ) : ControllerBase
    {
        private readonly InsuranceContext _context = context;

        // GET: api/Clients
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(inc => inc.ReleaseInformation)
                .Include(inc => inc.Diagnosis)
                .Include(inc => inc.PatientAccounts)
                .Include(x => x.Agreements)
                    .ThenInclude(x => x.Payroll)
                        .ThenInclude(x => x.Contractor)
                .Include(x => x.Agreements)
                    .ThenInclude(x => x.Payroll)
                        .ThenInclude(x => x.InsuranceProcedure)
                            .ThenInclude(x => x.Insurance)
                .Include(x => x.Agreements)
                    .ThenInclude(x => x.Payroll)
                        .ThenInclude(x => x.InsuranceProcedure)
                            .ThenInclude(x => x.Procedure)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var existingClient = await _context.Clients.Include(c => c.Agreements).FirstOrDefaultAsync(c => c.Id == id);

            if (existingClient == null)
            {
                return NotFound();
            }

            // Actualizar el client con los valores del nuevo client
            _context.Entry(existingClient).CurrentValues.SetValues(client);

            if (client.Agreements != null)
            {
                client.Agreements.ForEach(x => x.Payroll = null);

                foreach (var aggrement in client.Agreements)
                {
                    if (aggrement.Id == 0)
                    {
                        existingClient.Agreements.Add(aggrement);
                    }
                    else
                    {
                        var existingAgreement = existingClient.Agreements!.FirstOrDefault(a => a.Id == aggrement.Id);
                        if (existingAgreement != null)
                        {
                            _context.Entry(existingAgreement).CurrentValues.SetValues(aggrement);
                        }
                    }
                }
            }

            if (existingClient.Agreements != null)
            {
                // Identificar y eliminar los aggreemnt que fueron eliminados del Client
                foreach (var existingAggrement in existingClient.Agreements.ToList())
                {
                    if (!client.Agreements.Any(p => p.Id == existingAggrement.Id))
                    {
                        // El Agg existente no está presente en la lista enviada, por lo tanto, eliminarlo
                        _context.Agreements.Remove(existingAggrement);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            client.Diagnosis = null;
            client.ReleaseInformation = null;

            if (client.Agreements != null)
                client.Agreements.ForEach(x => x.Payroll = null);

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get all client of the contractor and insurance
        [HttpGet("GetClientsByContractorAndInsurance/{contractorId}/{insuranceId}")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsByContractorAndInsurance(int contractorId, int insuranceId)
        {
            return await _context.Clients
                .Include(x => x.Agreements)
                    .ThenInclude(x => x.Payroll)
                        .ThenInclude(x => x.Contractor)
                .Include(x => x.Agreements)
                    .ThenInclude(x => x.Payroll)
                        .ThenInclude(x => x.InsuranceProcedure)
                            .ThenInclude(x => x.Insurance)
                .Include(x => x.Agreements)
                    .ThenInclude(x => x.Payroll)
                        .ThenInclude(x => x.InsuranceProcedure)
                            .ThenInclude(x => x.Procedure)
                .Where(x => x.Agreements.Any(a => a.Payroll.ContractorId == contractorId && a.Payroll.InsuranceProcedure.InsuranceId == insuranceId))
                .ToListAsync();
        }   
        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
