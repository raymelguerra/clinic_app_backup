using ClinicApp.Core.Models;

namespace ClinicApp.MSServiceLog.Dtos
{
    public class ServiceLogsDto
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual ClientDto Client { get; set; } = null!;

        public virtual ContractorDto Contractor { get; set; } = null!;

        public virtual PeriodDto Period { get; set; } = null!;
    }
}
