using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class Contractor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? RenderingProvider { get; set; }

    public bool Enabled { get; set; }

    public string? Extra { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual ICollection<ServiceLog> ServiceLogs { get; set; } = new List<ServiceLog>();
    public virtual ContractorUser? ContractorUser { get; set; }
}
