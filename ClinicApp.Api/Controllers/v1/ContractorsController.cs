using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using MediatR;
using ClinicApp.Infrastructure.Commands;
using ClinicApp.Infrastructure.Dtos.Application;
using ClinicApp.Infrastructure.Queries;

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class ContractorsController(InsuranceContext context, IMediator mediator) : ControllerBase
    {
        private readonly InsuranceContext _context = context;
        private readonly IMediator _mediator = mediator;


        // GET: api/Contractors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllContractorsResponse>>> GetContractors()
        {
            // return await _context.Contractors.ToListAsync();
            var command = new ContractorGetAllQuery();
            var response = await _mediator.Send(command);
            return Ok(response);
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
        public async Task<ActionResult<CreateContractorResponse>> PostContractor(CreateContractorRequest contractor)
        {
            //_context.Contractors.Add(contractor);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetContractor", new { id = contractor.Id }, contractor);

            var command = new ContractorCreateCommand(contractor);
            var response = await _mediator.Send(command);
            return Ok(response);
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

        // Get contractors by procedure and insurance
        [HttpGet("procedure/{procedureId}/insurance/{insuranceId}")]
        public async Task<ActionResult<IEnumerable<Contractor>>> GetContractorsByProcedureAndInsurance(int procedureId, int insuranceId)
        {
            var contractors = await _context.Contractors
     .Include(c => c.Payrolls)
         .ThenInclude(p => p.InsuranceProcedure)
             .ThenInclude(ip => ip.Insurance)
     .Include(c => c.Payrolls)
         .ThenInclude(p => p.InsuranceProcedure)
             .ThenInclude(ip => ip.Procedure)
     .Where(c => c.Payrolls.Any(p =>
         p.InsuranceProcedure.ProcedureId == procedureId &&
         p.InsuranceProcedure.InsuranceId == insuranceId))
     .ToListAsync();
            return Ok(contractors);
        }
        private bool ContractorExists(int id)
        {
            return _context.Contractors.Any(e => e.Id == id);
        }
    }
}
