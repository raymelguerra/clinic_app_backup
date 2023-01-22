namespace ClinicApp.MSBilling.Dtos
{
    public class SetServiceLogBilledRequest
    {
        public int PeriodId { get; set; }
        public int ContratorId { get; set; }
        public int ClientId { get; set; }
        public string? UserId { get; set; }
    }
}
