using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;

namespace ClinicApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public IssuesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issues>>> GetIssues()
        {
            return await _context.Issues.ToListAsync();
        }

        // GET: api/Issues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Issues>> GetIssues(int id)
        {
            var issues = await _context.Issues.FindAsync(id);

            if (issues == null)
            {
                return NotFound();
            }

            return issues;
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssues(int id, Issues issues)
        {
            if (id != issues.Id)
            {
                return BadRequest();
            }

            _context.Entry(issues).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssuesExists(id))
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

        // POST: api/Issues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Issues>> PostIssues(Issues issues)
        {
            _context.Issues.Add(issues);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssues", new { id = issues.Id }, issues);
        }

        // DELETE: api/Issues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssues(int id)
        {
            var issues = await _context.Issues.FindAsync(id);
            if (issues == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issues);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssuesExists(int id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }
    }
}
