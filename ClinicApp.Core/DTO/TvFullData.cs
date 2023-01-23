using ClinicApp.Core.Models;

namespace ClinicApp.Core.DTO
{
    public class TvFullData
    {
        public Client client { get; set; }
        public Contractor contractor { get; set; }
        public ContractorType contractorType { get; set; }
        public PatientAccount patientAccount { get; set; }
        public ServiceLog serviceLog { get; set; }
        public UnitDetail unitDetail { get; set; }
        public SubProcedure subProcedure { get; set; }
    }
}