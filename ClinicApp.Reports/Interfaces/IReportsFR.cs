using ClinicApp.Infrastructure.Dto;
using Microsoft.AspNetCore.Components;

namespace ClinicApp.Reports.Interfaces
{
    public interface IReportsFR
    {
        public Task<byte[]> GetMonthlyAbsenteeReportFRAAsync(IEnumerable<MontlhyAbsenteeReportDto> data);
    }
}
