using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class UnitDetail
    {
        public int Id { get; set; }
        public string? Modifiers { get; set; }
        public int PlaceOfServiceId { get; set; }
        public DateTime DateOfService { get; set; }
        public int Unit { get; set; }
        public int ServiceLogId { get; set; }
        public int SubProcedureId { get; set; }

        public virtual PlaceOfService PlaceOfService { get; set; } = null!;
        public virtual ServiceLog ServiceLog { get; set; } = null!;
        public virtual SubProcedure SubProcedure { get; set; } = null!;
    }
}
