using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.DTO;

public  class TvServiceLog : TvObject
{
    public string Id { get; set; }

    public string Name { get { return $"{CreatedDate}"; } set { } }

    public string Status { get; set; }

    public DateTime? CreatedDate { get; set; }
    
    public TvContractor Contractor { get; set; }
}
