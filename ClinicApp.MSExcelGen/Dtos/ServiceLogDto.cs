using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class ServiceLogDto
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? BilledDate { get; set; }
        public string? Biller { get; set; }
        public string? Pending { get; set; }
        public ClientDto Client { get; set; } = null!;
        public ContractorDto Contractor { get; set; } = null!;
        //public virtual PeriodDto Period { get; set; } = null!;
    }
}