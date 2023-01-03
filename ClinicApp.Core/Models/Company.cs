using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class Company
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Acronym { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<Agreement> Agreements { get; } = new List<Agreement>();

    public virtual ICollection<Payroll> Payrolls { get; } = new List<Payroll>();
}
