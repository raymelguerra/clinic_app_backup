using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;

[Route("api/infrastructure/[controller]")]
[ApiController]
public class PlaceOfServiceController : ControllerBase
{
    private readonly IPlaceOfService _placeOfService;

    public PlaceOfServiceController(IPlaceOfService placeOfService)
    {
        _placeOfService = placeOfService;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaceOfService>>> GetPlaceOfService()
    {
        return Ok(await _placeOfService.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PlaceOfService>> GetPlaceOfService(int id)
    {
        var placeOfService = await _placeOfService.Get(id);

        if (placeOfService == null)
        {
            return NotFound();
        }

        return placeOfService;
    }
}
