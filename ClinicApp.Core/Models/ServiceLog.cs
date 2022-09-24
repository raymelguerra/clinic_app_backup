using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class ServiceLog
    {
        public ServiceLog()
        {
            UnitDetails = new HashSet<UnitDetail>();
        }

        public int Id { get; set; }
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Contractor Contractor { get; set; } = null!;
        public virtual Period Period { get; set; } = null!;
        public virtual ICollection<UnitDetail> UnitDetails { get; set; }
    }
}
