using ClinicApp.Core.Models;

namespace ClinicApp.MSServiceLog.Dtos
{
    public class ServiceLogByNameDto : ServiceLogsDto
    {
        public int Id { get; set; }

        public int PeriodId { get; set; }

        public int ContractorId { get; set; }

        public int ClientId { get; set; }

        public DateTime? BilledDate { get; set; }

        public string? Biller { get; set; }

        public string? Pending { get; set; }
    }
}
