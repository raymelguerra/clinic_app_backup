using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;

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
        public async Task<ActionResult<IEnumerable<ServiceLog>>> GetServiceLogs()
        {
            return await _context.ServiceLogs.ToListAsync();
        }

        // GET: api/ServiceLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceLog>> GetServiceLog(int id)
        {
            var serviceLog = await _context.ServiceLogs.FindAsync(id);

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

            _context.Entry(serviceLog).State = EntityState.Modified;

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
