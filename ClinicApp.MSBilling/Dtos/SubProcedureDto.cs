namespace ClinicApp.MSBilling.Dtos
{
    public class SubProcedureDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public double Rate { get; set; }

        public int ProcedureId { get; set; }
    }
}