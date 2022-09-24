using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Services;

public class PlaceOfServiceService : IPlaceOfService
{
    private readonly clinicbdContext _context;
    public PlaceOfServiceService(clinicbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlaceOfService>> Get()
    {
        return await _context.PlaceOfServices.ToListAsync();
    }

    public async Task<PlaceOfService?> Get(int id)
    {
        var placeOfService = await _context.PlaceOfServices.FindAsync(id);

        if (placeOfService == null)
        {
            return null;
        }

        return placeOfService;
    }
}
