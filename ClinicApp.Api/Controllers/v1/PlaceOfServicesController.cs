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
    public class PlaceOfServicesController : ControllerBase
    {
        private readonly InsuranceContext _context;

        public PlaceOfServicesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: api/PlaceOfServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceOfService>>> GetPlacesOfService()
        {
            return await _context.PlacesOfService.ToListAsync();
        }

        // GET: api/PlaceOfServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaceOfService>> GetPlaceOfService(int id)
        {
            var placeOfService = await _context.PlacesOfService.FindAsync(id);

            if (placeOfService == null)
            {
                return NotFound();
            }

            return placeOfService;
        }

        // PUT: api/PlaceOfServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaceOfService(int id, PlaceOfService placeOfService)
        {
            if (id != placeOfService.Id)
            {
                return BadRequest();
            }

            _context.Entry(placeOfService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceOfServiceExists(id))
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

        // POST: api/PlaceOfServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlaceOfService>> PostPlaceOfService(PlaceOfService placeOfService)
        {
            _context.PlacesOfService.Add(placeOfService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaceOfService", new { id = placeOfService.Id }, placeOfService);
        }

        // DELETE: api/PlaceOfServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaceOfService(int id)
        {
            var placeOfService = await _context.PlacesOfService.FindAsync(id);
            if (placeOfService == null)
            {
                return NotFound();
            }

            _context.PlacesOfService.Remove(placeOfService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaceOfServiceExists(int id)
        {
            return _context.PlacesOfService.Any(e => e.Id == id);
        }
    }
}
