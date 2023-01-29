namespace ClinicApp.MSBilling.Dtos
{
    public class ServiceLogDto
    {
        public int Id { get; set; }

        public int PeriodId { get; set; }

        public int ContractorId { get; set; }

        public int ClientId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? BilledDate { get; set; }

        public string? Pending { get; set; }
    }
}