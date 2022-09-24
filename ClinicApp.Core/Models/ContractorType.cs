using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class ContractorType
    {
        public ContractorType()
        {
            Payrolls = new HashSet<Payroll>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Payroll> Payrolls { get; set; }
    }
}
