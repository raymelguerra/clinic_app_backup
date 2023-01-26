using System;

namespace ClinicApp.Core.DTO;

public class ExtendedPeriod
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? PayPeriod { get; set; }
}