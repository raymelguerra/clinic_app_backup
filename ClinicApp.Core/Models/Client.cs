using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? RecipientId { get; set; }

    public string? PatientAccount { get; set; }

    public int ReleaseInformationId { get; set; }

    public string? ReferringProvider { get; set; }

    public string? AuthorizationNumber { get; set; }

    public int Sequence { get; set; }

    public int DiagnosisId { get; set; }

    public bool Enabled { get; set; }

    public int WeeklyApprovedRbt { get; set; }

    public int WeeklyApprovedAnalyst { get; set; }

    public virtual ICollection<Agreement> Agreements { get; set; } = new List<Agreement>();

    public virtual Diagnosis Diagnosis { get; set; } = null!;

    public virtual ICollection<PatientAccount> PatientAccounts { get; } = new List<PatientAccount>();

    public virtual ReleaseInformation ReleaseInformation { get; set; } = null!;

    public virtual ICollection<ServiceLog> ServiceLogs { get; set; } = new List<ServiceLog>();
}
