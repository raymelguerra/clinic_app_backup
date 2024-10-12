using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.OData.Query;
using System.Security.Claims;
using Oauth2.sdk.Models;

namespace ClinicApp.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceLogsController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public ServiceLogsController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/ServiceLogs
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ServiceLog>>> GetServiceLogs()
        {
            return await _context.ServiceLogs
                .Include(x => x.Insurance)
                .Include(x => x.Client)
                .Include(x => x.Contractor)
                .Include(x => x.Period)
                .ToListAsync();
        }

        // GET: api/ServiceLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceLog>> GetServiceLog(int id)
        {
            var serviceLog = await _context.ServiceLogs
                .Include(x => x.UnitDetails)
                    .ThenInclude(x => x.Procedure)
                .Include(x => x.UnitDetails)
                    .ThenInclude(x => x.PlaceOfService)
                .Include(x => x.Insurance)
                .Include(x => x.Client)
                    .ThenInclude(x => x.Agreements)
                        .ThenInclude(x => x.Payroll)
                            .ThenInclude( x=> x.InsuranceProcedure)
                                .ThenInclude(x => x.Procedure)
                .Include(x => x.Contractor)
                .Include(x => x.Period)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (serviceLog == null)
            {
                return NotFound();
            }

            return serviceLog;
        }

        // PUT: api/ServiceLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceLog(int id, ServiceLog serviceLog)
        {
            if (id != serviceLog.Id)
            {
                return BadRequest();
            }

            var existingServiceLog = await _context.ServiceLogs
                .Include(c => c.UnitDetails).ThenInclude(ud => ud.Procedure)
                .Include(c => c.UnitDetails).ThenInclude(ud => ud.PlaceOfService)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingServiceLog == null)
            {
                return NotFound();
            }

            _context.Entry(existingServiceLog).CurrentValues.SetValues(serviceLog);

            if (serviceLog.UnitDetails != null && serviceLog.UnitDetails.Count() > 0)
            {
                serviceLog.UnitDetails.ForEach(x => x.Procedure = null);
                serviceLog.UnitDetails.ForEach(x => x.PlaceOfService = null);

                foreach (var unitDetail in serviceLog.UnitDetails)
                {
                    if (unitDetail.Id == 0)
                    {
                        existingServiceLog.UnitDetails.Add(unitDetail);
                    }
                    else
                    {
                        var existingUnitDetails = existingServiceLog.UnitDetails.FirstOrDefault(p => p.Id == unitDetail.Id);
                        if (existingUnitDetails != null)
                        {
                            // Actualizar el Unit Detail existente
                            _context.Entry(existingUnitDetails).CurrentValues.SetValues(unitDetail);
                        }
                    }
                }
            }

            if (existingServiceLog.UnitDetails != null && existingServiceLog.UnitDetails.Count() > 0)
            {
                foreach (var existingUnitDetail in existingServiceLog.UnitDetails.ToList())
                {
                    if (!serviceLog.UnitDetails.Any(p => p.Id == existingUnitDetail.Id))
                    {
                        _context.UnitDetails.Remove(existingUnitDetail);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLogExists(id))
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

        // POST: api/ServiceLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceLog>> PostServiceLog(ServiceLog serviceLog)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            serviceLog.Client = null;
            serviceLog.Contractor = null;
            serviceLog.Insurance = null;
            serviceLog.Period = null;
            serviceLog.Biller = userId ?? "";
            serviceLog.BilledDate = DateTime.Now;
            serviceLog.Pending = "";
            serviceLog.CreatedDate = DateTime.Now;

            _context.ServiceLogs.Add(serviceLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceLog", new { id = serviceLog.Id }, serviceLog);
        }

        // DELETE: api/ServiceLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceLog(int id)
        {
            var serviceLog = await _context.ServiceLogs.FindAsync(id);
            if (serviceLog == null)
            {
                return NotFound();
            }

            _context.ServiceLogs.Remove(serviceLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceLogExists(int id)
        {
            return _context.ServiceLogs.Any(e => e.Id == id);
        }
    }
}
