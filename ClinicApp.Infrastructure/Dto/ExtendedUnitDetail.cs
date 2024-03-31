using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Dto;

public class ExtendedUnitDetail
{
    public UnitDetail? unitDetail { get; set; }
    public ServiceLog? serviceLog { get; set; }
    public Procedure? Procedure { get; set; }
    public PlaceOfService? placeOfService { get; set; }
    public PatientAccount? patientAccount { get; set; }
}