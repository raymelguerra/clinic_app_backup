using ClinicApp.Infrastructure.Dto;
using FastReport;
using FastReport.Export.PdfSimple;
using ClinicApp.Reports.Interfaces;
using ClinicApp.Reports.Utils;

namespace ClinicApp.Reports.Services
{
    public class ReportsFRServices : IReportsFR
    {
        private readonly Report _report;
        public ReportsFRServices()
        {
            _report = new Report();
        }
        private const string REPORT_PATH = "ReportsTemplates";
        public async Task<byte[]> GetMonthlyAbsenteeReportFRAAsync(IEnumerable<MontlhyAbsenteeReportDto> data)
        {

            _report.Load($"{REPORT_PATH}/MonthlyAbsenteeReport.frx");

            if (data == null || data.Count() == 0)
            {
                _report.RegisterData(data, "MontlhyAbsenteeReportRef");
                _report.SetParameterValue("Month", 0);
            }
            else
            {
                _report.RegisterData(data, "MontlhyAbsenteeReportRef");
                _report.SetParameterValue("Month", data.FirstOrDefault().Month);
            }

            return await BuildReport();
        }

        private async Task<byte[]> BuildReport()
        {
            if (_report.Prepare())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    _report.Export(pdfExport, ms);

                    ms.Position = 0;

                    byte[] result = new byte[ms.Length];
                    await ms.ReadAsync(result, 0, (int)ms.Length);

                    _report.Dispose();
                    pdfExport.Dispose();

                    return result;
                }
            }
            else
            {
                return [];
            }
        }
    }
}
