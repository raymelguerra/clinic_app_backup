using ClinicApp.Core.Models;

namespace ClinicApp.Core.DTO
{
    public class ExtendedAgrUnitDetail
    {
        public ServiceLog? serviceLog { get; set; }
        public UnitDetail? unitDetail { get; set; }
        public Agreement? agreement { get; set; }
    }
}