using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedure _procedure;

    public ProcedureController(IProcedure procedure)
    {
        _procedure = procedure;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Procedure>>> GetProcedure()
    {
        return Ok(await _procedure.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Procedure>> GetProcedure(int id)
    {
        var procedure = await _procedure.Get(id);

        if (procedure == null)
        {
            return NotFound();
        }

        return procedure;
    }
}
