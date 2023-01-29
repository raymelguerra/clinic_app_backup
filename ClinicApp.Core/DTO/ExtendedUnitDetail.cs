using ClinicApp.Core.Models;

namespace ClinicApp.Core.DTO;

public class ExtendedUnitDetail
{
    public UnitDetail? unitDetail { get; set; }
    public ServiceLog? serviceLog { get; set; }
    public SubProcedure? subProcedure { get; set; }
    public PlaceOfService? placeOfService { get; set; }
    public PatientAccount? patientAccount { get; set; }
}