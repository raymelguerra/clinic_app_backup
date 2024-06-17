using ClinicApp.Infrastructure.Dto;
using FastReport;
using FastReport.Export.PdfSimple;
using ClinicApp.Reports.Interfaces;
using ClinicApp.Reports.Utils;

namespace ClinicApp.Reports.Services
{
    public class ReportsFRServices : IReportsFR
    {
        const string REPORT_PATH = "ReportsTemplates/MonthlyAbsenteeReport.frx";
        public async Task<byte[]> GetMonthlyAbsenteeReportFRAAsync(IEnumerable<MontlhyAbsenteeReportDto> data)
        {
            Report report = new Report();

            report.Load(REPORT_PATH);

            if (data == null || data.Count() == 0)
            {
                report.RegisterData(data, "MontlhyAbsenteeReportRef");
                report.SetParameterValue("Month", 0);
            }
            else
            {
                report.RegisterData(data, "MontlhyAbsenteeReportRef");
                report.SetParameterValue("Month", data.FirstOrDefault().Month);
            }

            if (report.Prepare())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    report.Export(pdfExport, ms);

                    ms.Position = 0;

                    byte[] result = new byte[ms.Length];
                    await ms.ReadAsync(result, 0, (int)ms.Length);

                    report.Dispose();
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
