//using ClinicApp.Core.Models;

using ClinicApp.Core.Models;

namespace ClinicApp.Core.DTO
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
        //public Client client { get; set; }
        //public Contractor contractor { get; set; }
        //public ContractorType contractorType { get; set; }
        //public PatientAccount patientAccount { get; set; }
        //public ServiceLog serviceLog { get; set; }
        //public UnitDetail unitDetail { get; set; }
        //public SubProcedure subProcedure { get; set; }
    }
}