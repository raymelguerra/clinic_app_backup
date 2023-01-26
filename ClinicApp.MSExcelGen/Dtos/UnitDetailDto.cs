using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class UnitDetailDto
    {
        public int Id { get; set; }

        public string? Modifiers { get; set; }

        public int PlaceOfServiceId { get; set; }

        public DateTime DateOfService { get; set; }

        public int Unit { get; set; }

        public int ServiceLogId { get; set; }

        public int SubProcedureId { get; set; }

        public virtual PlaceOfServiceDto PlaceOfService { get; set; } = null!;

        public virtual ServiceLogDto ServiceLog { get; set; } = null!;

        public virtual SubProcedureDto SubProcedure { get; set; } = null!;
    }
}