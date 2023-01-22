using ClinicApp.Core.Models;

namespace ClinicApp.Core.DTO
{
    public class ExtendedContractor
    {
        public Contractor? contractor { get; set; }
        public Company? company { get; set; }
    }
}