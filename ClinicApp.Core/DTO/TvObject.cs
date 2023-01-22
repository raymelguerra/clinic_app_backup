using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.DTO;

public interface TvObject
{
    string Id { get; set; }
    string Name { get; set; }
    string Status { get; set; }
}
