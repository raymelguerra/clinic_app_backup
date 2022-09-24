using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ClinicApp.Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosesController : ControllerBase
{
    private readonly IDiagnoses _diagnoses;

    public DiagnosesController(IDiagnoses diagnoses)
    {
        _diagnoses = diagnoses;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Diagnosis>>> GetDiagnosis()
    {
        var t = ConfigurationManager.ConnectionStrings.ToString();
        return Ok(await _diagnoses.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Diagnosis>> GetDiagnosis(int id)
    {
        var company = await _diagnoses.Get(id);

        if (company == null)
        {
            return NotFound();
        }

        return company;
    }
}
