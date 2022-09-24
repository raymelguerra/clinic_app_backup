using ClinicApp.MSClient.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.MSClient.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClient _client;
    public ClientController(IClient client)
    {
        _client = client;
    }
    // GET: api/<clientController>
    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<Client>>>> Get([FromQuery] PaginationFilter filter)
    {
        try
        {
            var route = Request.Path.Value ?? String.Empty;
            var client = await _client.GetClient(filter, route);
            return Ok(client);
        }
        catch (Exception e) 
        {
            return BadRequest(e.Message);
        }
    }

    // GET api/<ClientController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> Get(int id)
    {
        try 
        {
            var client = await _client.GetClient(id);
            return Ok(client);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/Clients
    [HttpGet("GetClientByName/{name}")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientByName([FromQuery] PaginationFilter filter, string name) 
    {
        try
        {
            var route = Request.Path.Value!;
            var clients = await _client.GetClientByName(filter, name, route);
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetClientsByContractor/{id}")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientsByContractor(int id) 
    {
        try
        {
            var clients = await _client.GetClientsByContractor(id);
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // GET: api/Clients/GetClientWithoutDetails
    [HttpGet("GetClientWithoutDetails")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientWithoutDetails() 
    {
        try
        {
            var clients = await _client.GetClientWithoutDetails();
            return Ok(clients);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // POST api/<ClientController>
    [HttpPost]
    public async Task<ActionResult<Client>> Post(Client client)
    {
        try
        {
            var created = await _client.PostClient(client);
            return Ok(created);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // PUT api/<ClientController>/5
    [HttpPut("{id}"), Authorize(Roles = "Administrator, Biller")]
    public async Task<IActionResult> Put(int id, Client client)
    {
        if (id != client.Id)
        {
            return BadRequest();
        }
        try
        {
            var created = await _client.PutClient(id, client);
            if (created == null)
                return NotFound();
            
            return NoContent();
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // DELETE api/<ClientController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _client.DeleteClient(id);
        if(client == null)
           return NotFound();
        return NoContent();

    }
}
