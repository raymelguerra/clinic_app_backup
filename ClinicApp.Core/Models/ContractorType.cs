using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class ContractorType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; } = new List<Payroll>();
}
