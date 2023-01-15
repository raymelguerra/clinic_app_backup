using ClinicApp.Core.Models;

namespace ClinicApp.MSClient.Dtos
{
    public class ClientCreateDto
    {
        public string? Name { get; set; }

        public string? RecipientId { get; set; }

        public string? PatientAccount { get; set; }

        public int ReleaseInformationId { get; set; }

        public string? ReferringProvider { get; set; }

        public string? AuthorizationNumber { get; set; }

        public int Sequence { get; set; }

        public int DiagnosisId { get; set; }

        public bool Enabled { get; set; }

        public int WeeklyApprovedRbt { get; set; }

        public int WeeklyApprovedAnalyst { get; set; }

        public virtual ICollection<Agreement> Agreements { get; set; } = new List<Agreement>();

        public virtual ICollection<PatientAccount> PatientAccounts { get; } = new List<PatientAccount>();
    }
}
