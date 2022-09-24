using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Biller.Interfaces
{
    public interface IBilling
    {
        public Task<PagedResponse<IEnumerable<Billing>>> GetBilling([FromQuery] PaginationFilter filter, string route);
        public Task<Billing?> GetBilling(int id);
        public Task<object?> PutBilling(int id, Billing biller);
        public Task<Billing?> PostBilling(Billing biller);
        public Task<object?> DeleteBilling(int id);
        public bool BillingExists(int id);
    }
}
