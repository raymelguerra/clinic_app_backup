using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Models;
using ClinicApp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;

[Route("api/infrastructure/[controller]")]
[ApiController]
public class PeriodController : ControllerBase
{
    private readonly IPeriod _period;

    public PeriodController(IPeriod period)
    {
        _period = period;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Period>>> GetPeriod()
    {
        return Ok(await _period.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Period>> GetPeriod(int id)
    {
        var period = await _period.Get(id);

        if (period == null)
        {
            return NotFound();
        }

        return period;
    }

    [HttpGet("GetDataPeriod/{id_period}/{id_client}")]
    public async Task<ActionResult<DataPeriodDto>> GetDataPeriod(int id_period, int id_client) 
    {
        try
        {
            var periodData = await _period.GetDataPeriod(id_period, id_client);
            return Ok(periodData);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // GET: api/Periods/GetActivePeriod Obtener el periodo activo
    [HttpGet("GetActivePeriod")]
    public async Task<ActionResult<Period>> GetActivePeriod() 
    {
        try
        {
            return Ok(await _period.GetActivePeriod());
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

}
