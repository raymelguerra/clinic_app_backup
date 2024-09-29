
namespace ClinicApp.Infrastructure.Dtos.Application
{
    public class GetProcedureByIdResponse
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ContractorTypeId { get; set; }
    }
}
