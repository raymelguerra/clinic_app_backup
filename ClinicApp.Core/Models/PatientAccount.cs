using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class PatientAccount
{
    public int Id { get; set; }

    public string? LicenseNumber { get; set; }

    public string? Auxiliar { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public int ClientId { get; set; }

    public virtual Client? Client { get; set; }
}
