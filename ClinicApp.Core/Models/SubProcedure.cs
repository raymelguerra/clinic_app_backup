using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class SubProcedure
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Rate { get; set; }

    public int ProcedureId { get; set; }

    public virtual Procedure Procedure { get; set; } = null!;

    public virtual ICollection<UnitDetail> UnitDetails { get; } = new List<UnitDetail>();
}
