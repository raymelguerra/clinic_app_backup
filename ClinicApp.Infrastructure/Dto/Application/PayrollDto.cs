namespace ClinicApp.Infrastructure.Dtos.Application
{
    public class CreatePayrollRequest
    {
        public int ContractorTypeId { get; set; }
        public int InsuranceProcedureId { get; set; }
        public int CompanyId { get; set; }
    }
    public class CreatePayrollResponse
    {
        public int Id { get; set; } = 0;
        public int ContractorId { get; set; } = 0;
        public int ContractorTypeId { get; set; } = 0;
        public int InsuranceProcedureId { get; set; } = 0;
        public int CompanyId { get; set; } = 0;
    }

    public class GetPayrollByIdResponse
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int ContractorTypeId { get; set; }
        public int InsuranceProcedureId { get; set; }
        public int CompanyId { get; set; }

        public ContractorTypeGetByIdResponse? ContractorType { get; set; } = null!;
        public GetInsuranceProcedureByIdResponse? InsuranceProcedure { get; set; }
        public GetCompanyByIdResponse? Company { get; set; }
    }
}
