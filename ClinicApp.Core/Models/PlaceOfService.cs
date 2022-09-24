using System;
using System.Collections.Generic;

namespace ClinicApp.Core.Models
{
    public partial class PlaceOfService
    {
        public PlaceOfService()
        {
            UnitDetails = new HashSet<UnitDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }

        public virtual ICollection<UnitDetail> UnitDetails { get; set; }
    }
}
