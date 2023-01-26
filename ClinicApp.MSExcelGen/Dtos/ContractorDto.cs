using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class ContractorDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RenderingProvider { get; set; }
        public string? Extra { get; set; }
    }
}