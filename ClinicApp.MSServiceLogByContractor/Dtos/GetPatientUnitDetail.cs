namespace ClinicApp.MSServiceLogByContractor.Dtos;
public class GetPatientUnitDetail
{
    public int Id { get; set; }
    public int UnitDetailId { get; set; }
    public string? Signature { get; set; }
    public string? EntryTime { get; set; }
    public string? DepartureTime { get; set; }
    public int ProcedureId { get; set; }
    public int PlaceOfServiceId { get; set; }
}
