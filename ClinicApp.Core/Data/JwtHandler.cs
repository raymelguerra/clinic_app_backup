using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicApp.Core.Data;

public class JwtHandler
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly UserManager<IdentityUser> _userManager;
    public JwtHandler(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");
    }

    public async Task<List<Claim>> GetClaims(IdentityUser user)
    {
        var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName)
};
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
}
