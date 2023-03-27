namespace ClinicApp.MSServiceLogByContractor.Dtos
{
	public class CreateUnitDetail
	{
        // Original Unit details
        public string? Modifiers { get; set; }
        public int PlaceOfServiceId { get; set; }
        public DateTime DateOfService { get; set; }
        public int Unit { get; set; }
        public int SubProcedureId { get; set; }
        public int ServiceLogId { get; set; }

        // Patient Unit details
        public virtual int UnitDetailId { get; set; }
        public string? PatientSignature { get; set; }
        public string? EntryTime { get; set; }
        public string? DepartureTime { get; set; }
        public DateTime? PatientSignatureDate { get; set; }
    }
}
