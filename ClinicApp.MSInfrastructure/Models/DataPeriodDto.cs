using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Models
{
    public class DataPeriodDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<ServiceLog>? ServiceLog { get; set; }
    }
}
