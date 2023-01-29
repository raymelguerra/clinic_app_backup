using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;

[Route("api/infrastructure/[controller]")]
[ApiController]
public class ReleaseInformationController : ControllerBase
{
    private readonly IReleaseInformation _releaseInformation;

    public ReleaseInformationController(IReleaseInformation releaseInformation)
    {
        _releaseInformation = releaseInformation;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReleaseInformation>>> GetReleaseInformation()
    {
        return Ok(await _releaseInformation.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ReleaseInformation>> GetReleaseInformation(int id)
    {
        var releaseInformation = await _releaseInformation.Get(id);

        if (releaseInformation == null)
        {
            return NotFound();
        }

        return releaseInformation;
    }
}
