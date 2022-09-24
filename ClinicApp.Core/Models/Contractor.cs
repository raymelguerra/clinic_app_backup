using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class Contractor
    {
        public Contractor()
        {
            Billings = new HashSet<Billing>();
            Payrolls = new HashSet<Payroll>();
            ServiceLogs = new HashSet<ServiceLog>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RenderingProvider { get; set; }
        public bool Enabled { get; set; }
        public string? Extra { get; set; }

        public virtual ICollection<Billing> Billings { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
        public virtual ICollection<ServiceLog> ServiceLogs { get; set; }
    }
}
