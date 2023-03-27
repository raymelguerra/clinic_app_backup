using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Core.DTO
{
    public class CreateServiceLog
    {
        public DateTime? SignatureDateContractor { get; set; }
        public DateTime? SignatureDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? BilledDate { get; set; }
        public DateTime DateOfService { get; set; }
        public int ServiceLogId { get; set; }
        
        public string? SignatureContractor { get; set; }
        public string? SignatureUnitDetail { get; set; }
        public string? EntryTime { get; set; }
        public string? DepartureTime { get; set; }
        public int PeriodId { get; set; }
        public int ContractorId { get; set; }
        public int ClientId { get; set; }
        public string? Biller { get; set; }
        public string? Pending { get; set; }
        public int Status { get; set; }
        public string? Modifiers { get; set; }
        
        public int PlaceOfServiceId { get; set; }
        public int Unit { get; set; }
        public int SubProcedureId { get; set; }

    }
}
