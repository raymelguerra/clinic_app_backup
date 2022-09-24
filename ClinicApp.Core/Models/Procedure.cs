using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class Procedure
    {
        public Procedure()
        {
            Payrolls = new HashSet<Payroll>();
            SubProcedures = new HashSet<SubProcedure>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public double Rate { get; set; }

        public virtual ICollection<Payroll> Payrolls { get; set; }
        public virtual ICollection<SubProcedure> SubProcedures { get; set; }
    }
}
