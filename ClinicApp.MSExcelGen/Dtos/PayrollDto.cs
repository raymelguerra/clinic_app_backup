using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class PayrollDto
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int ContractorTypeId { get; set; }
        public int ProcedureId { get; set; }
        public ContractorDto Contractor { get; set; } = null!;
        public ContractorTypeDto ContractorType { get; set; } = null!;
        public ProcedureDto Procedure { get; set; } = null!;
    }
}