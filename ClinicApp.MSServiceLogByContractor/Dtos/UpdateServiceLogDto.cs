namespace ClinicApp.MSServiceLogByContractor.Dtos
{
    public class UpdateServiceLogDto
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
    
        public string? Signature { get; set; }
        public DateTime? SignatureDate { get; set; }
      
        public List<NewUnitDetailDto> UnitDetails { get; set; } = null!;
    }

}
