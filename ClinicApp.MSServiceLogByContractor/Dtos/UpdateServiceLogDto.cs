namespace ClinicApp.MSServiceLogByContractor.Dtos
{
    public class UpdateServiceLogDto
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Pending { get; set; }
        public int Status { get; set; }
        public string? Signature { get; set; }
        public DateTime? SignatureDate { get; set; }
        public IEnumerable<UpdateUnitDetail> UnitDetails { get; set; } = null!;
    }

}
