using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSExcelGen.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.MSExcelGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelGenController : ControllerBase
    {
        private readonly IExcelGen _excelGen;

        public ExcelGenController(IExcelGen excelGen)
        {
            _excelGen = excelGen;
        }

        [HttpGet("GetCompanies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies() {
            try
            {
                return await _excelGen.GetCompanies();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetPeriodsAsync")]
        public async Task<ActionResult<IEnumerable<ExtendedPeriod>>> GetPeriodsAsync()
        {
            try
            {
                return await _excelGen.GetPeriodsAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("GetPeriodAsync/{periodId}")]
        public async Task<ActionResult<Period>> GetPeriodAsync(int periodId = -1)
        {
            try
            {
                return await _excelGen.GetPeriodAsync(periodId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("GetExContractorsAsync/{companyCode}")]
        public async Task<ActionResult<IEnumerable<ExtendedContractor>>> GetExContractorsAsync(string companyCode)
        {
            try
            {
                return await _excelGen.GetExContractorsAsync(companyCode);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("GetAgreementsAsync/{contractorId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<Agreement>>> GetAgreementsAsync(int contractorId, int companyId)
        {
            try
            {
                return await _excelGen.GetAgreementsAsync(contractorId, companyId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 
        
        [HttpGet("GetExAgrUnitDetails/{clientId}/{contractorId}/{companyId}/{periodId}")]
        public async Task<ActionResult<IEnumerable<ExtendedAgrUnitDetail>>> GetExAgrUnitDetails(int clientId, int contractorId, int companyId, int periodId)
        {
            try
            {
                return await _excelGen.GetExAgrUnitDetails(clientId, contractorId, companyId, periodId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
