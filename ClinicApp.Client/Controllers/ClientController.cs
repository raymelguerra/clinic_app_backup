using ClinicApp.MSClient.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using ClinicApp.MSClient.Dtos;

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
        try
        {
            var client = await _client.DeleteClient(id);
            if (client == null)
                return NotFound();
            return NoContent();
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }

    // GET: api/Agreements
    [HttpGet("Agreement")]
    public async Task<ActionResult<IEnumerable<Agreement>>> GetAgreement([FromQuery] int? clientIdFilter)
    {
        var ag = await _client.GetAgreement(clientIdFilter);
        return Ok(ag);
    }

    // GET: api/Agreements/5
    [HttpGet("Agreement/GetAgreementByContractor/{id}")]
    public async Task<ActionResult<IEnumerable<Agreement>>> GetAgreementByContractor(int id)
    {
        var agreements = await _client.GetAgreementByContractor(id);

        if (agreements == null)
        {
            return NotFound();
        }

        return Ok(agreements);
    }

    // GET: api/Agreements/5
    [HttpGet("Agreement/{id}")]
    public async Task<ActionResult<Agreement>> GetAgreement(int id)
    {
        var agreement = await _client.GetAgreement(id);

        if (agreement == null)
        {
            return NotFound();
        }

        return Ok(agreement);
    }

    // PUT: api/Agreements/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("Agreement/{id}")]
    public async Task<IActionResult> PutAgreement(int id, Agreement agreement)
    {
        if (id != agreement.Id)
        {
            return BadRequest();
        }
        try
        {
            var created = await _client.PutAgreement(id, agreement);
            if (created == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/Agreements
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("Agreement")]
    public async Task<ActionResult<Agreement>> PostAgreement(Agreement agreement)
    {
        try
        {
            var created = await _client.PostAgreement(agreement);
            return Ok(created);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // DELETE: api/Agreements/5
    [HttpDelete("Agreement/{id}")]
    public async Task<IActionResult> DeleteAgreement(int id)
    {
        var agreement = await _client.DeleteAgreement(id);
        if (agreement == null)
            return NotFound();
        return NoContent();
    }

    // GET: api/PatientAccount
    [HttpGet("PatientAccount")]
    public async Task<ActionResult<IEnumerable<PatientAccount>>> GetBilling()
    {
        return Ok(await _client.GetBilling());
    }

    // GET: api/PatientAccount/5
    [HttpGet("PatientAccount/{id}")]
    public async Task<ActionResult<PatientAccount>> GetPatientAccount(int id)
    {
        var patient = await _client.GetPatientAccount(id);

        if (patient == null)
        {
            return NotFound();
        }

        return patient;
    }

    // GET: api/PatientAccount/idClient
    [HttpGet("PatientAccount/byclient/{idclient}")]
    public async Task<ActionResult<IEnumerable<PatientAccount>>> GetBilling(int idclient)
    {
        return Ok(await _client.GetBilling(idclient));
    }

    // PUT: api/PatientAccount/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("PatientAccount/{id}"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> PutPatientAccount(int id, PatientAccount patient)
    {
        if (id != patient.Id)
        {
            return BadRequest();
        }

        patient.LicenseNumber = patient.LicenseNumber ?? "DOES NOT APPLY";
        try
        {
            var created = await _client.PutPatientAccount(id, patient);
            if (created == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    // POST: api/PatientAccount
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("PatientAccount"), Authorize(Roles = "Administrator")]
    public async Task<ActionResult<PatientAccount>> PostPatientAccount(PatientAccount patient)
    {
        try
        {
            var created = await _client.PostPatientAccount(patient);
            return Ok(created);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // DELETE: api/PatientAccount/5
    [HttpDelete("PatientAccount/{id}"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var patient_account = await _client.DeletePatientAccount(id);
        if (patient_account == null)
            return NotFound();
        return NoContent();
    }
}
