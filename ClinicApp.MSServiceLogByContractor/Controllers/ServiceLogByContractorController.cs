using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;
using ClinicApp.MSServiceLogByContractor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

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

        [HttpGet("all/{ContractorId}")]
        public async Task<ActionResult<IEnumerable<AllServiceLogDto>>> GetAll(int ContractorId)
        {
            try
            {
                var sl = await _service.GetAllAsync(ContractorId);
                _logger.LogInformation($"Event: {LogEvent.GET_ALL}    Datetime {DateTime.Now.ToLocalTime()}");
                return Ok(sl);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Event: {LogEvent.GET_ALL}  Datetime {DateTime.Now.ToLocalTime()}  Message {e.Message}");
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
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"Event: {LogEvent.GET_BY_ID}  Datetime {DateTime.Now.ToLocalTime()}  Message {e.Message}");
                return NotFound(e.Message);
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
        public async Task<ActionResult<GetContractorServiceLogDto>> Put(int id, [FromBody] UpdateServiceLogDto sl)
        {
            try
            {
                var updatedSl = await _service.UpdateAsync(id, sl);
                _logger.LogInformation($"Event: {LogEvent.UPDATED}    Datetime {DateTime.Now.ToLocalTime()}");
                return Ok(updatedSl);
            }
            catch (Exception e)
            {
                _logger.LogError($"Event: {LogEvent.UPDATED}    Datetime {DateTime.Now.ToLocalTime()}");
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<ServiceLogByContractorController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation($"Event: {LogEvent.DELETED}    Datetime {DateTime.Now.ToLocalTime()}");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Event: {LogEvent.DELETED}    Datetime {DateTime.Now.ToLocalTime()}");
                return BadRequest(e.Message);
            }

        }

        // POST api/<ServiceLogByContractorController>
        [HttpPost("createuser")]
        public async Task<ActionResult> CreateUserContractorPost([FromBody] CreateUserContractor value)
        {
            try
            {
                var sl = await _service.CreateUserContractorAsync(value);
                if (sl != 1) {
                    _logger.LogInformation($"Event: {LogEvent.NOT_FOUND}    Datetime {DateTime.Now.ToLocalTime()}");
                    return NotFound();
                }
                _logger.LogInformation($"Event: {LogEvent.CREATED}    Datetime {DateTime.Now.ToLocalTime()}");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Event: {LogEvent.CREATED}    Datetime {DateTime.Now.ToLocalTime()}");
                return BadRequest(e.Message);
            }
        }
    }
}
