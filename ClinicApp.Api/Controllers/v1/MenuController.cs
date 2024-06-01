using Asp.Versioning;
using ClinicApp.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oauth2.sdk;

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class MenuController(IMenusService _menuService, IUserManagementService _userManagement) : ControllerBase
    {
        private readonly IMenusService menuService = _menuService;
        private readonly IUserManagementService userManagement = _userManagement;

        [HttpGet("{role}")]
        public IActionResult GetMenus(string role)
        {
            //var claims = HttpContext.User.Claims.ToList();

            //foreach (var claim in claims)
            //{
            //    Console.WriteLine($"{claim.Type}: {claim.Value}");
            //}

            //string roleString = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "resource_access")?.Value!;

            //var roleContainer = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(roleString);

            //string role = "";
            //if (roleContainer!.TryGetValue("clinicapp-api", out var clinicAppData) && clinicAppData.TryGetValue("roles", out var roles) && roles.Count > 0)
            //    role = roles.FirstOrDefault()!;


            //if (role == "" || role == "guest")
            //    return Unauthorized();

            var menus = menuService.GetMenusByRole(role);

            if (menus == null)
                return NotFound();

            return Ok(menus);
        }
    }
}
