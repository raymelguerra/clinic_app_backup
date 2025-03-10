﻿using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class Payroll
{
    public int Id { get; set; }

    public int ContractorId { get; set; }

    public int ContractorTypeId { get; set; }

    public int ProcedureId { get; set; }

    public int CompanyId { get; set; }

    public virtual ICollection<Agreement> Agreements { get; } = new List<Agreement>();

    public virtual Company Company { get; set; } = null!;

    public virtual Contractor Contractor { get; set; } = null!;

    public virtual ContractorType ContractorType { get; set; } = null!;

    public virtual Procedure Procedure { get; set; } = null!;
}
