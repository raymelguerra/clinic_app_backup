using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class Billing
    {
        public int Id { get; set; }
        public DateTime BillingDate { get; set; }
        public int ClientId { get; set; }
        public int ContractorId { get; set; }
        public int PeriodId { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Contractor Contractor { get; set; } = null!;
        public virtual Period Period { get; set; } = null!;
    }
}
