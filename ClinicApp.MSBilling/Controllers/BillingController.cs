using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSBilling.Dtos;
using ClinicApp.MSBilling.Interfaces;
using ClinicApp.MSBilling.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Data;
using System.Security.Principal;

namespace ClinicApp.MSBilling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBilling _billing;
        public BillingController(IBilling billingService)
        {
            _billing = billingService;
        }
        [HttpGet("GetPeriods")]
        public async Task<ActionResult<IEnumerable<ExtendedPeriod>>> GetPeriodsAsync()
        {
            var periods = await _billing.GetPeriodsAsync();
            return Ok(periods);
        }
        [HttpGet("GetPeriod/{periodId}")]
        public async Task<ActionResult<Period>> GetPeriodAsync(int periodId)
        {
            var period = await _billing.GetPeriodAsync(periodId);
            return Ok(period);
        }

        [HttpGet("GetCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompaniesAsync()
        {
            var companies = await _billing.GetCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("GetContractorAndClients/{companyCode}/{periodId}")]
        public async Task<ActionResult<IEnumerable<TvClient>>> GetContractorAndClientsAsync(string companyCode, int periodId)
        {
            var result = await _billing.GetContractorAndClientsAsync(companyCode, periodId);
            return Ok(result);
        }

        [HttpGet("GetAgreement/{companyCode}/{periodId}/{contractorId}/{clientId}")]
        public async Task<ActionResult<Agreement>> GetAgreementAsync(string companyCode, int periodId, int contractorId, int clientId)
        {
            var result = await _billing.GetAgreementAsync(companyCode, periodId, contractorId, clientId);
            return Ok(result);
        }

        [HttpGet("GetExUnitDetails/{periodId}/{contractorId}/{clientId}/{paccount}/{sufixList}")]
        public async Task<ActionResult<IEnumerable<ExtendedUnitDetail>>> GetExUnitDetailsAsync(int periodId, int contractorId, int clientId, string paccount, string sufixList)
        {
            var result = await _billing.GetExUnitDetailsAsync(periodId, contractorId, clientId, paccount, sufixList);
            return Ok(result);
        }

        [HttpGet("GetExUnitDetailsAux/{periodId}/{contractorId}/{clientId}/{paccount}/{sufixList}")]
        public async Task<ActionResult<IEnumerable<ExtendedUnitDetail>>> GetExUnitDetailsAuxAsync(int serviceLogId, string pAccount, string sufixList)
        {
            {
                var result = await _billing.GetExUnitDetailsAsync(serviceLogId, pAccount, sufixList);
                return Ok(result);
            }
        }

        [HttpGet("GetExServiceLog/{companyCode}/{serviceLogId}")]
        public async Task<ActionResult<ExtendedServiceLog>> GetExServiceLogAsync(string companyCode, int serviceLogId) {
            var result = await _billing.GetExServiceLogAsync(companyCode, serviceLogId);
            return Ok(result);
        }

        [HttpPut("SetServiceLogBilled/{serviceLogId}")]
        public async Task<IActionResult> SetServiceLogBilled(int serviceLogId, [FromBody]Dictionary<string,string> user) {
            var result = await _billing.SetServiceLogBilled(serviceLogId, user["userId"]);
            if (result != null)
                return NoContent();
            else {
                return BadRequest();
            }
        }
        
        [HttpPut("SetServiceLogBilled")]
        public async Task<IActionResult> SetServiceLogBilled([FromBody] SetServiceLogBilledRequest body) {
            var result = await _billing.SetServiceLogBilled(body.PeriodId, body.ContratorId, body.ClientId, body.UserId!);
            if (result != null)
                return NoContent();
            else {
                return BadRequest();
            }
        }

        [HttpPut("SetServiceLogPendingReason")]
        public async Task<IActionResult> SetServiceLogPendingReason(int serviceLogId, [FromBody] Dictionary<string, string> reason)
        {
            var result = await _billing.SetServiceLogBilled(serviceLogId, reason["reason"]);
            if (result != null)
                return NoContent();
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateBilling/{serviceLogId}")]
        public async Task<IActionResult> UpdateBilling(int serviceLogId)
        {
            var result = await _billing.UpdateBilling(serviceLogId);
            if (result != null)
                return NoContent();
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetServiceLogsBilled/{period}/{company}")]
        public async Task<ActionResult<IEnumerable<ManagerBiller>>> GetServiceLogsBilled(int period, int company)
        {
            var result = await _billing.GetServiceLogsBilled(period, company);
            return Ok(result);
        }
    }
}
