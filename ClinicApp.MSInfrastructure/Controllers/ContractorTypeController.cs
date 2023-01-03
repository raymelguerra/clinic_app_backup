using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.MSInfrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;
[Route("api/infrastructure/[controller]")]
[ApiController]
public class ContractorTypeController : ControllerBase
{
    private readonly IContractorType _contractorType;

    public ContractorTypeController(IContractorType contractorType)
    {
        _contractorType = contractorType;
    }


    // GET: api/ContractorType
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractorType>>> GetContractorType()
    {
        return Ok(await _contractorType.Get());
    }

    // GET: api/Companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ContractorType>> GetContractorType(int id)
    {
        var contractorType = await _contractorType.Get(id);

        if (contractorType == null)
        {
            return NotFound();
        }

        return contractorType;
    }
}
