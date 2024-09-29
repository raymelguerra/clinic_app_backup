

namespace ClinicApp.Infrastructure.Dtos.Application
{
    public class GetInsuranceByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? EntryDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class  GetInsuranceProcedureByIdResponse
    {
        public int Id { get; set; }
        public int InsuranceId { get; set; }
        public int ProcedureId { get; set; }
        public double Rate { get; set; }
        public GetInsuranceByIdResponse Insurance { get; set; } = null!;
        public GetInsuranceByIdResponse Procedure { get; set; } = null!;
    }
}
