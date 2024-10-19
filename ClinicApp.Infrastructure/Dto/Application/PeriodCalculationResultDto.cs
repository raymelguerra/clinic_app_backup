using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Infrastructure.Dto.Application
{
    public class PeriodCalculationResultDto
    {
        public decimal BilledToInsurance { get; set; }
        public decimal AmountPaidToContractors { get; set; }
        public decimal Profit { get; set; }
    }

}
