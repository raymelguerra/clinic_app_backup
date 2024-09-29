using Asp.Versioning;
using ClinicApp.Infrastructure.Dto;
using ClinicApp.Infrastructure.Persistence;
using ClinicApp.Reports.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class ReportsController(
        InsuranceContext context,
        IReportsFR reportsFRServices
        ) : ControllerBase
    {
        private readonly InsuranceContext _context = context;
        private readonly IReportsFR ReportsFRServices = reportsFRServices;

        [HttpGet("montlhy-absentee-report")]
        public ActionResult<IEnumerable<MontlhyAbsenteeReportDto>> GetMontlhyAbsenteeReport(
            [FromQuery] int Month = 0)
        {
            if (Month == 0)
            {
                Month = DateTime.Now.Month;
            }
            var result = _context.ServiceLogs
                .Include(sl => sl.Contractor)
                .Where(sl => sl.CreatedDate!.Value.Month == Month)
                .GroupBy(sl => new { sl.ContractorId })
                .Select(g => new MontlhyAbsenteeReportDto
                {
                    Month = DateTime.Now.ToString("MMMM"),
                    PhysicianName = g.First().Contractor.Name,
                    RenderingProvider = g.First().Contractor.RenderingProvider ?? "Not present",
                    Extra = g.First().Contractor.Extra,
                    AttendanceCount = g.First().UnitDetails!.Select(ud => ud.DateOfService!.Value.Date).Distinct().Count(),
                })
                .AsEnumerable();

            return Ok(result);
        }


        [HttpPost("montlhy-absentee-report/download")]
        public async Task<ActionResult> GetMontlhyAbsenteeReportDownload(
            [FromBody] IEnumerable<MontlhyAbsenteeReportDto> data)
        {
            var pdfBytes = await ReportsFRServices.GetMonthlyAbsenteeReportFRAAsync(data);
            // Verifica si se generaron los bytes del PDF
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                return NotFound("El reporte no se pudo generar.");
            }

            MemoryStream stream = new MemoryStream(pdfBytes);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = "report.pdf"
            };
        }
    }
}
