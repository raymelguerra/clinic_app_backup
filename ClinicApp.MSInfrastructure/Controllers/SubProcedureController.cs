using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;
[Route("api/infrastructure/[controller]")]
[ApiController]
public class SubProcedureController : ControllerBase
{
    private readonly ISubProcedure _subprocedure;

    public SubProcedureController(ISubProcedure subprocedure)
    {
        _subprocedure = subprocedure;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubProcedure>>> GetSubProcedure()
    {
        return Ok(await _subprocedure.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SubProcedure>> GetSubProcedure(int id)
    {
        var subprocedure = await _subprocedure.Get(id);

        if (subprocedure == null)
        {
            return NotFound();
        }

        return subprocedure;
    }

    // GET: api/Procedures
    [HttpGet("GetSubProceduresByAgreement/{clientId}/{contractorId}")]
    public async Task<ActionResult<IEnumerable<SubProcedure>>> GetSubProceduresByAgreement(int clientId, int contractorId)
    {
        var data = await _subprocedure.GetSubProceduresByAgreement(clientId, contractorId);
        if (data != null)
            return Ok(data);
        return BadRequest("Error in input data");
    }
}
