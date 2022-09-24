using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class PatientAccount
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string ExpireDate { get; set; } = null!;
        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}
