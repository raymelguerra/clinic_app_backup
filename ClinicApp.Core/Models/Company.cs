using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class Company
    {
        public Company()
        {
            Agreements = new HashSet<Agreement>();
            Payrolls = new HashSet<Payroll>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Acronym { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<Agreement> Agreements { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
    }
}
