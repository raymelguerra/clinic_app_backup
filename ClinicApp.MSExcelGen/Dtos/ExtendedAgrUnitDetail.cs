using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class ExtendedAgrUnitDetail
    {
        public ServiceLogDto serviceLog { get; set; }
        public UnitDetailDto unitDetail { get; set; }
        public AgreementDto agreement { get; set; }
    }
}