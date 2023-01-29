using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;
[Route("api/infrastructure/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanies _companies;

    public CompaniesController(ICompanies companies)
    {
        _companies = companies;
    }


    // GET: api/Companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
    {
        return Ok(await _companies.Get());
    }

    // GET: api/Companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(int id)
    {
        var company = await _companies.Get(id);

        if (company == null)
        {
            return NotFound();
        }

        return company;
    }
}
