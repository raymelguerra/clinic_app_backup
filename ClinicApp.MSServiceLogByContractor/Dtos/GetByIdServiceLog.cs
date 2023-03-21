namespace ClinicApp.MSServiceLogByContractor.Dtos
{
    public class GetByIdServiceLog
    {
        public int Id { get; set; }
        public GetContractorServiceLogDto? ContractorServiceLogDto { get; set; }
        public int ClientId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Pending { get; set; }

    }
}
