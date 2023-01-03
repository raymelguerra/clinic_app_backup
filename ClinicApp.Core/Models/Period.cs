using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models;

public partial class Period
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool Active { get; set; }

    public string PayPeriod { get; set; } = null!;

    public DateTime DocumentDeliveryDate { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual ICollection<ServiceLog> ServiceLogs { get; } = new List<ServiceLog>();
}
