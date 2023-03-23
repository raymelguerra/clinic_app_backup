using ClinicApp.Core.Models;

namespace ClinicApp.MSServiceLogByContractor.Dtos
{
	public class CreateServiceLogDto
	{
        // Original Service log
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Pending { get; set; }
        public int Status { get; set; }

        // Contractor service log
        public string? Signature { get; set; }
        public DateTime? SignatureDate { get; set; }


        public IEnumerable<CreateUnitDetail> UnitDetails { get; set; } = null!;

    }
}
