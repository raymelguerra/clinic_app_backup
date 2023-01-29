using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class ClientDto
    {
        public int Id { get; set; }

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

        public DiagnosisDto Diagnosis { get; set; } = null!;

        public ReleaseInformationDto ReleaseInformation { get; set; } = null!;
    }
}