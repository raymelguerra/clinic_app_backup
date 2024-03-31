using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;

namespace ClinicApp.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReleaseInformationsController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public ReleaseInformationsController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/ReleaseInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReleaseInformation>>> GetReleaseInformations()
        {
            return await _context.ReleaseInformations.ToListAsync();
        }

        // GET: api/ReleaseInformations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReleaseInformation>> GetReleaseInformation(int id)
        {
            var releaseInformation = await _context.ReleaseInformations.FindAsync(id);

            if (releaseInformation == null)
            {
                return NotFound();
            }

            return releaseInformation;
        }

        // PUT: api/ReleaseInformations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReleaseInformation(int id, ReleaseInformation releaseInformation)
        {
            if (id != releaseInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(releaseInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReleaseInformationExists(id))
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

        // POST: api/ReleaseInformations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReleaseInformation>> PostReleaseInformation(ReleaseInformation releaseInformation)
        {
            _context.ReleaseInformations.Add(releaseInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReleaseInformation", new { id = releaseInformation.Id }, releaseInformation);
        }

        // DELETE: api/ReleaseInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReleaseInformation(int id)
        {
            var releaseInformation = await _context.ReleaseInformations.FindAsync(id);
            if (releaseInformation == null)
            {
                return NotFound();
            }

            _context.ReleaseInformations.Remove(releaseInformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReleaseInformationExists(int id)
        {
            return _context.ReleaseInformations.Any(e => e.Id == id);
        }
    }
}
