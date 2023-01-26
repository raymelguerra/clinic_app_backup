using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Dtos
{
    public class AgreementDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public int PayrollId { get; set; }
        public double RateEmployees { get; set; }
        //public Client Client { get; set; } = null!;
        public CompanyDto Company { get; set; } = null!;
        public PayrollDto Payroll { get; set; } = null!;
    }
}
