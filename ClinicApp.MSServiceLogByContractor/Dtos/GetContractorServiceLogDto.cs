namespace ClinicApp.MSServiceLogByContractor.Dtos
{
    public class GetContractorServiceLogDto
    {
        public int Id { get; set; }
        public string? Signature { get; set; }
        public IEnumerable<GetPatientUnitDetail>? PatientUnitDetails { get; set; }
    }
}
