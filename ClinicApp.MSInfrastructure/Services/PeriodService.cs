using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ClinicApp.Infrastructure.Services
{
    public class PeriodService : IPeriod
    {
        private readonly clinicbdContext _context;
        public PeriodService(clinicbdContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Period>> Get()
        {
            return await _context.Periods.ToListAsync();
        }

        public async Task<Period?> Get(int id)
        {
            var period = await _context.Periods.FindAsync(id);

            if (period == null)
            {
                return null;
            }

            return period;
        }

        public async Task<Period?> GetActivePeriod()
        {
            var period = await _context.Periods.Where(x => x.Active == true).FirstOrDefaultAsync();

            if (period == null)
            {
                return null;
            }

            return period;
        }

        public async Task<DataPeriodDto?> GetDataPeriod(int id_period, int id_client)
        {
            var data = await _context.Clients.Where(w => w.Id == id_client).Select(c =>  new DataPeriodDto
            {
                Id = c.Id,
                Name = c.Name,
                ServiceLog = _context.ServiceLogs.Include("Contractor").Include("Period").Where(sl => sl.ClientId == id_client && sl.PeriodId == id_period).Select(x => new ServiceLog
                {
                    Contractor = x.Contractor,
                    UnitDetails = _context.UnitDetails.Include("SubProcedure").Include("PlaceOfService").Where(ud => ud.ServiceLogId == x.Id).OrderByDescending(ud => ud.DateOfService).ToList()
                }).ToList()
            }).FirstOrDefaultAsync();

            if (data == null)
            {
                return null;
            }

            return data;
        }
    }
}
