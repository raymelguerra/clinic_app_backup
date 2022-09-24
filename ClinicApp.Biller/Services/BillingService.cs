using ClinicApp.Biller.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ClinicApp.Biller.Services
{
    public class BillingService : IBilling
    {
        private readonly clinicbdContext _context;
        private readonly IUriService _uriService;
        public BillingService(clinicbdContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        public bool BillingExists(int id)
        {
            return _context.Billings.Any(e => e.Id == id);
        }

        public async Task<object?> DeleteBilling(int id)
        {
            var biller = await _context.Billings.FindAsync(id);
            if (biller == null)
            {
                return null;
            }

            _context.Billings.Remove(biller);
            await _context.SaveChangesAsync();

            return new Billing { };
        }

        public async Task<PagedResponse<IEnumerable<Billing>>> GetBilling([FromQuery] PaginationFilter filter, string route)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var list = await _context.Billings
                .Include(x => x.Client)
                .Include(x => x.Contractor)
                .Include(x => x.Period)
                .OrderBy(o => o.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await _context.Billings.CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<Billing>(list, validFilter, totalRecords, _uriService, route);
            return pagedReponse;
        }

        public async Task<Billing?> GetBilling(int id)
        {
            var biller = await _context.Billings.FindAsync(id);
            if (biller == null)
            {
                return null;
            }

            return biller;
        }

        public async Task<Billing?> PostBilling(Billing biller)
        {
            _context.Billings.Add(biller);
            await _context.SaveChangesAsync();

            return await _context.Billings
                .Include(x => x.Client)
                .Include(x => x.Contractor)
                .Include(x => x.Period)
                .FirstOrDefaultAsync(x => x.Id == biller.Id);
        }

        public async Task<object?> PutBilling(int id, Billing biller)
        {
            _context.Entry(biller).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillingExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return new Billing();
        }
    }
}
