using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class Procedure
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double Rate { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; } = new List<Payroll>();

    public virtual ICollection<SubProcedure> SubProcedures { get; } = new List<SubProcedure>();
}
