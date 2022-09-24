using ClinicApp.Biller.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.Biller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBilling _billing;
        public BillingController(IBilling billing)
        {
            _billing = billing;
        }
        // GET: api/<BillingController>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<Billing>>>> Get([FromQuery] PaginationFilter filter)
        {
            try
            {
                var route = Request.Path.Value ?? String.Empty;
                var biller = await _billing.GetBilling(filter, route);
                return Ok(biller);
            }
            catch (Exception e) 
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/<BillingController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> Get(int id)
        {
            try 
            {
                var biller = await _billing.GetBilling(id);
                return Ok(biller);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<BillingController>
        [HttpPost]
        public async Task<ActionResult<Billing>> Post(Billing biller)
        {
            try
            {
                var created = await _billing.PostBilling(biller);
                return Ok(created);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<BillingController>/5
        [HttpPut("{id}"), Authorize(Roles = "Administrator, Biller")]
        public async Task<IActionResult> Put(int id, Billing biller)
        {
            if (id != biller.Id)
            {
                return BadRequest();
            }
            try
            {
                var created = await _billing.PutBilling(id, biller);
                if (created == null)
                    return NotFound();
                
                return NoContent();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // DELETE api/<BillingController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var biller = await _billing.DeleteBilling(id);
            if(biller == null)
               return NotFound();
            return NoContent();

        }
    }
}
