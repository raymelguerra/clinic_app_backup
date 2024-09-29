using Asp.Versioning;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class PayrollController(InsuranceContext context) : ControllerBase
    {
        private readonly InsuranceContext _context = context;

        // Get All payrolls based on insuranceId and procedureId
        [HttpGet("insurance/{insuranceId}")]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetPayrollsByInsuranceAndProcedure(int insuranceId)
        {
            var payrolls = await _context.Payrolls
                .Where(p => p.InsuranceProcedure.InsuranceId == insuranceId)
                .Include(p => p.Contractor)
                .Include(p => p.ContractorType)
                .Include(p => p.InsuranceProcedure)
                .Include(p => p.Company)
                .ToListAsync();

            return payrolls;
        }

        // Get all payrolls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetPayrolls()
        {
            return await _context.Payrolls
                .Include(p => p.Contractor)
                .Include(p => p.ContractorType)
                .Include(p => p.InsuranceProcedure)
                    .ThenInclude(ip => ip.Insurance)
                .Include(i => i.InsuranceProcedure)
                    .ThenInclude(ip => ip.Procedure)
                .Include(p => p.Company)
                .ToListAsync();
        }
    }
}
