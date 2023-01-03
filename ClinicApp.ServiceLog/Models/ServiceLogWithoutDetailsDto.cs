namespace ClinicApp.MSServiceLog.Models
{
    public class ServiceLogWithoutDetailsDto
    {
        public string? ClientName { get; set; }
        public string? ContractorName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? OrderBY { get; set; }
    }
}
