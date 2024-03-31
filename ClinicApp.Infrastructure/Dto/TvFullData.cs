

namespace ClinicApp.Infrastructure.Dto
{
    public class TvFullData
    {        
        public int clientId { get; set; }
        public string? clientName { get; set; }
        public int contractorId { get; set; }
        public string? contractorName { get; set; }
        public string? contractorTypeName { get; set; }
        public string? patientAccountAuxiliar { get; set; }
        public string? patientAccountLicenseNumber { get; set; }
        public int serviceLogId { get; set; }
        public DateTime? serviceLogCreatedDate { get; set; }
        public DateTime? serviceLogBilledDate { get; set; }

    }
}