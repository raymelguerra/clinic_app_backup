using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.Data
{
    public class AuthSettings
    {
         public string? Key { get; set; }
         public string? Audience { get; set; }
        public string? Issuer { get; set; }
    }
}
