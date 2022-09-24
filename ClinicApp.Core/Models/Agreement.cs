using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class Agreement
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public int PayrollId { get; set; }
        public double RateEmployees { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Company Company { get; set; } = null!;
        public virtual Payroll Payroll { get; set; } = null!;
    }
}
