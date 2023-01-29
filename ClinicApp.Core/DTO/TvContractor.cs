using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.DTO;

public class TvContractor : TvObject
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string ContratorType { get; set; }

    public string Status
    {
        get
        {
            if (ServiceLogs.All(x => x.Status == "billed")) return "billed";
            else if (ServiceLogs.Any(x => x.Status == "billed")) return "billed_gray";
            return "empty";
        }
        set { }
    }

    public TvClient Client { get; set; }
            
    public List<TvServiceLog> ServiceLogs { get; set; }

    public TvContractor()
    {
        ServiceLogs = new List<TvServiceLog>();
    }
}
