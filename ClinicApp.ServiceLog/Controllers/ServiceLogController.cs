using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLog.Dtos;
using ClinicApp.MSServiceLog.Interfaces;
using ClinicApp.MSServiceLog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.MSServiceLog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceLogController : ControllerBase
{
    private readonly IServiceLog _serviceLog;
    public ServiceLogController(IServiceLog serviceLog)
    {
        _serviceLog = serviceLog;
    }
    // GET: api/<clientController>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<AllServicesLogDto>>>> Get([FromQuery] PaginationFilter filter)
    {
        try
        {
            var route = Request.Path.Value ?? String.Empty;
            var contractor = await _serviceLog.GetServiceLog(filter, route);
            return Ok(contractor);
        }
        catch (Exception e) 
        {
            return BadRequest(e.Message);
        }
    }

    // GET api/<ServiceLogController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceLog>> Get(int id)
    {
        try 
        {
            var contractor = await _serviceLog.GetServiceLog(id);
            return Ok(contractor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/ServiceLogs
    [HttpGet("GetServiceLogByName/{name}/{type}")]
    public async Task<ActionResult<IEnumerable<ServiceLog>>> GetServiceLogByName([FromQuery] PaginationFilter filter, string name, string type) 
    {
        try
        {
            var route = Request.Path.Value!;
            var clients = await _serviceLog.GetServiceLogByName(filter, name, route, type);
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // GET: api/ServiceLogs/GetServiceLogWithoutDetails
    [HttpGet("GetServiceLogWithoutDetails")]
    public async Task<ActionResult<IEnumerable<ServiceLogWithoutDetailsDto>>> GetServiceLogWithoutDetails() 
    {
        try
        {
            var clients = await _serviceLog.GetServiceLogWithoutDetails();
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // POST api/<ServiceLogController>
    [HttpPost]
    public async Task<ActionResult<ServiceLog>> Post(ServiceLog contractor)
    {
        try
        {
            var created = await _serviceLog.PostServiceLog(contractor);
            return Ok(created);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // PUT api/<ServiceLogController>/5
    [HttpPut("{id}"), Authorize(Roles = "Administrator, Biller")]
    public async Task<IActionResult> Put(int id, ServiceLog contractor)
    {
        if (id != contractor.Id)
        {
            return BadRequest();
        }
        try
        {
            var created = await _serviceLog.PutServiceLog(id, contractor);
            if (created == null)
                return NotFound();
            
            return NoContent();
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // DELETE api/<ServiceLogController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contractor = await _serviceLog.DeleteServiceLog(id);
        if(contractor == null)
           return NotFound();
        return NoContent();

    }

    // GET: api/ServiceLogs
    [HttpGet("GetPendingServiceLog"), Authorize(Roles = "Administrator,Operator")]
    public async Task<ActionResult<IEnumerable<ServiceLog>>> GetPendingServiceLog([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;
        var pagedReponse = await _serviceLog.GetPendingServiceLog(filter, route!);
        return Ok(pagedReponse);
    }


    [HttpPatch("UpdatePendingStatus/{id}"), Authorize(Roles = "Administrator,Operator")]
    public async Task<ActionResult<ServiceLog>> UpdatePendingStatus(int id)
    {
        var response = await _serviceLog.UpdatePendingStatus(id);
        if (response == null)
            return NotFound();
        return NoContent();
    }

    [HttpGet("GetServiceLogsByName/{name}/{type}")]//, Authorize(Roles = "Administrator,Operator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<ServiceLogByNameDto>>>> GetServiceLogsByName([FromQuery] PaginationFilter filter, string name, string type)
    {
        var route = Request.Path.Value!;
        var pagedResponse = await _serviceLog.GetServiceLogsByName(filter, name, type, route);
            return Ok(pagedResponse);
    }
}
