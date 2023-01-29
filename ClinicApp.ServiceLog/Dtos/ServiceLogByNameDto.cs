using ClinicApp.Core.Models;

namespace ClinicApp.MSServiceLog.Dtos
{
    public class ServiceLogByNameDto : ServiceLogsDto
    {
        public DateTime? BilledDate { get; set; }

        public string? Biller { get; set; }

        public string? Pending { get; set; }
    }
}
