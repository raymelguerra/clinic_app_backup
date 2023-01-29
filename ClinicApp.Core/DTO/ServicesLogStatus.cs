using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.DTO;

[Keyless]
public class ServicesLogStatus
{
    public int? Billed { get; set; }
    public int? Pending { get; set; }
    public int? NotBilled { get; set; }
    public int Total { get { return this.Billed?? 0 + this.NotBilled?? 0 + this.Pending?? 0; } }  
    public object this[string propertyName]
    {
        get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
        set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
    }
}
