using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSDashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.MSDashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboard;
        public DashboardController(IDashboard dashboard)
        {
            _dashboard = dashboard;
        }
        // GET: api/Dashboard/Profit/{company_id}
        [HttpGet("Profit/{company_id}")]
        public async Task<ActionResult<IEnumerable<ProfitHistory>>> Get(int company_id)
        {
            try
            {
                var response = await _dashboard.GetProfit(company_id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/Dashboard/GetServicesLogStatus/{company_id}/{period_id}
        [HttpGet("GetServicesLogStatus/{company_id}/{period_id}")]
        public async Task<ActionResult<ServicesLogStatus>> GetServicesLogStatus(int company_id, int period_id)
        {
            try
            {
                var response = await _dashboard.GetServicesLgStatus(company_id, period_id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/Dashboard/GetSLPatientAcc/{company_id}/{period_id}
        [HttpGet("GetServiceLogWithoutPatientAccount/{company_id}/{period_id}")]
        public async Task<ActionResult<IEnumerable<ServiceLogWithoutPatientAccount>>> GetServiceLogWithoutPatientAccount(int company_id, int period_id) {

            try
            {
                var response = await _dashboard.GetServiceLogWithoutPatientAccount(company_id, period_id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/Dashboard/GetGeneralData/{company_id}/{period_id}
        [HttpGet("GetGeneralData/{company_id}/{period_id}")]
        public async Task<ActionResult<GeneralData>> GetGeneralData(int company_id, int period_id)
        {
            try
            {
                var response = await _dashboard.GetGeneralData(company_id, period_id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/Dashboard/GetCompanies
        [HttpGet("GetCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            try
            {
                var response = await _dashboard.GetCompanies();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/Dashboard/GetPeriods
        [HttpGet("GetPeriods")]
        public async Task<ActionResult<IEnumerable<Period>>> GetPeriods()
        {
            try
            {
                var response = await _dashboard.GetPeriods();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
