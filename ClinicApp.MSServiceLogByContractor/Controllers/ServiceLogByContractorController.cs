using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;
using ClinicApp.MSServiceLogByContractor.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.MSServiceLogByContractor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceLogByContractorController : ControllerBase
    {
        private readonly ILogger<ServiceLogByContractorController> _logger;
        private readonly IServiceLogByContractor _service;

        public ServiceLogByContractorController(ILogger<ServiceLogByContractorController> logger, IServiceLogByContractor service)
        {
            _logger = logger;
            this._service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<AllServiceLogDto>>>> Get([FromQuery] PaginationFilter filter)
        {
            try
            {
                var route = Request.Path.Value ?? String.Empty;
                var sl = await _service.GetAllAsync(filter, route);
                _logger.LogInformation($"Event: {LogEvent.GET_ALL}    Datetime {DateTime.Now.ToLocalTime()}");
                return Ok(sl);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Event: {LogEvent.GET_ALL}  Datetime {DateTime.Now.ToLocalTime() }  Message {e.Message}");
                return BadRequest(e.Message);
            }
        }

        // GET api/<ServiceLogByContractorController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetContractorServiceLogDto>> Get(int id)
        {
            try
            {
                var sl = await _service.GetByIdAsync(id);
                _logger.LogInformation($"Event: {LogEvent.GET_BY_ID}    Datetime {DateTime.Now.ToLocalTime()}");
                return Ok(sl);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Event: {LogEvent.GET_BY_ID}  Datetime {DateTime.Now.ToLocalTime()}  Message {e.Message}");
                return BadRequest(e.Message);
            }
        }

        // POST api/<ServiceLogByContractorController>
        [HttpPost]
        public async Task<ActionResult<GetContractorServiceLogDto>> Post([FromBody] CreateServiceLogDto value)
        {
            try
            {
                var sl = await _service.CreateAsync(value);
                _logger.LogInformation($"Event: {LogEvent.CREATED}    Datetime {DateTime.Now.ToLocalTime()}");
                return Ok(sl);
            }
            catch (Exception e)
            {
                _logger.LogError($"Event: {LogEvent.CREATED}    Datetime {DateTime.Now.ToLocalTime()}");
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ServiceLogByContractorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServiceLogByContractorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
