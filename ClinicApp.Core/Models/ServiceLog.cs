using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class ServiceLog
{
    public int Id { get; set; }

    public int PeriodId { get; set; }

    public int ContractorId { get; set; }

    public int ClientId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? BilledDate { get; set; }

    public string? Biller { get; set; }

    public string? Pending { get; set; }
    
    public int Status { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Contractor Contractor { get; set; } = null!;

    public virtual Period Period { get; set; } = null!;

    public virtual ICollection<UnitDetail> UnitDetails { get; set; } = new List<UnitDetail>();
    public virtual ContractorServiceLog ContractorServiceLog { get; set; } = null!;
}
