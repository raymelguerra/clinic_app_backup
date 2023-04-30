namespace ClinicApp.MSServiceLogByContractor.Dtos
{
    /* public class UpdateUnitDetail
     {
         public int Id { get; set; }
         public string Modifiers { get; set; }
         public int PlaceOfServiceId { get; set; }
         public DateTime DateOfService { get; set; }
         public int Unit { get; set; }
         public int SubProcedureId { get; set; }
         public int ServiceLogId { get; set; }
         public UpdatePatientUnitDetailDto PatientUnitDetail { get; set; }

     }
     public class UpdatePatientUnitDetailDto
     {
         public int Id { get; set; }
         public virtual int UnitDetailId { get; set; }
         public string PatientSignature { get; set; }
         public string EntryTime { get; set; }
         public string DepartureTime { get; set; }
         public DateTime PatientSignatureDate { get; set; }
     }*/

    public class NewUnitDetailDto
    {
        public DateTime DateOfService { get; set; }
        public int PlaceOfServiceId { get; set; }
        public int SubProcedureId { get; set; }
        public string EntryTime { get; set; }
        public string DepartureTime { get; set; }
        public string Signature { get; set; }
        public DateTime SignatureDate { get; set; }
    }
}
