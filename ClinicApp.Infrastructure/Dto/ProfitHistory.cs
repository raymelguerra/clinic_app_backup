using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Dto;

[Keyless]
public class ProfitHistory
{
    public string? PayPeriod { get; set; }
    public double Billed { get; set; }
    public double Payment { get; set; }
    public double Profit { get; set; }

    public object this[string propertyName]
    {
        get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
        set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
    }
}
