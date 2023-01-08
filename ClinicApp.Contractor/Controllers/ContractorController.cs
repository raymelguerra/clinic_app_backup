using ClinicApp.MSContractor.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.MSContractor.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContractorController : ControllerBase
{
    private readonly IContractor _contractor;
    public ContractorController(IContractor contractor)
    {
        _contractor = contractor;
    }
    // GET: api/<clientController>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<Contractor>>>> Get([FromQuery] PaginationFilter filter)
    {
        try
        {
            var route = Request.Path.Value ?? String.Empty;
            var contractor = await _contractor.GetContractor(filter, route);
            return Ok(contractor);
        }
        catch (Exception e) 
        {
            return BadRequest(e.Message);
        }
    }

    // GET api/<ContractorController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Contractor>> Get(int id)
    {
        try 
        {
            var contractor = await _contractor.GetContractor(id);
            return Ok(contractor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/Contractors
    [HttpGet("GetContractorByName/{name}")]
    public async Task<ActionResult<IEnumerable<Contractor>>> GetContractorByName([FromQuery] PaginationFilter filter, string name) 
    {
        try
        {
            var route = Request.Path.Value!;
            var clients = await _contractor.GetContractorByName(filter, name, route);
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // GET: api/Contractors/GetContractorWithoutDetails
    [HttpGet("GetContractorWithoutDetails")]
    public async Task<ActionResult<IEnumerable<Contractor>>> GetContractorWithoutDetails() 
    {
        try
        {
            var clients = await _contractor.GetContractorWithoutDetails();
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // GET: api/Contractors/GetAnalystByCompany
    [HttpGet("GetAnalystByCompany/{id}")]
    public async Task<ActionResult<IEnumerable<Contractor>>> GetAnalystByCompany(int id) 
    {
        try
        {
            var clients = await _contractor.GetAnalystByCompany(id);
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }        
    }

    // GET: api/Contractors/GetContractorByCompany
    [HttpGet("GetContractorByCompany/{id}")]
    public async Task<ActionResult<IEnumerable<Contractor>>> GetContractorByCompany(int id)
    {
        try
        {
            var clients = await _contractor.GetContractorByCompany(id);
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // POST api/<ContractorController>
    [HttpPost]
    public async Task<ActionResult<Contractor>> Post(Contractor contractor)
    {
        try
        {
            var created = await _contractor.PostContractor(contractor);
            return Ok(created);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // PUT api/<ContractorController>/5
    [HttpPut("{id}"), Authorize(Roles = "Administrator, Biller")]
    public async Task<IActionResult> Put(int id, Contractor contractor)
    {
        if (id != contractor.Id)
        {
            return BadRequest();
        }
        try
        {
            var created = await _contractor.PutContractor(id, contractor);
            if (created == null)
                return NotFound();
            
            return NoContent();
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // DELETE api/<ContractorController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contractor = await _contractor.DeleteContractor(id);
        if(contractor == null)
           return NotFound();
        return NoContent();

    }

    // GET: api/Payrolls
    [HttpGet("payroll")]
    public async Task<ActionResult<IEnumerable<Payroll>>> GetPayroll()
    {
        return Ok(await _contractor.GetPayroll());
    }

    // GET: api/Payrolls
    [HttpGet("payroll/GetPayrollsByContractorAndCompany/{idCo}/{idCont}")]
    public async Task<ActionResult<IEnumerable<Payroll>>> GetPayrollsByContractorAndCompany(int idCo, int idCont)
    {
        return Ok(await _contractor.GetPayrollsByContractorAndCompany(idCo, idCont));
    }

    // GET: api/Payrolls/5
    [HttpGet("payroll/{id}")]
    public async Task<ActionResult<Payroll>> GetPayroll(int id)
    {
        var payroll = await _contractor.GetPayroll(id);

        if (payroll == null)
        {
            return NotFound();
        }

        return Ok(payroll);
    }

    // PUT: api/Payrolls/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("payroll/{id}")]
    public async Task<IActionResult> PutPayroll(int id, Payroll payroll)
    {
        if (id != payroll.Id)
        {
            return BadRequest();
        }
        try
        {
            var created = await _contractor.PutPayroll(id, payroll);
            if (created == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // POST: api/Payrolls
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("payroll")]
    public async Task<ActionResult<Payroll>> PostPayroll(Payroll payroll)
    {
        try
        {
            var created = await _contractor.PostPayroll(payroll);
            return Ok(created);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // DELETE: api/Payrolls/5
    [HttpDelete("payroll/{id}")]
    public async Task<IActionResult> DeletePayroll(int id)
    {
        var payroll = await _contractor.DeletePayroll(id);
        if (payroll == null)
            return NotFound();
        return NoContent();
    }    
}
