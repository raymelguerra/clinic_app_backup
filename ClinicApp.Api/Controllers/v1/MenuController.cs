using Asp.Versioning;
using ClinicApp.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class MenuController(IMenusService _menuService) : ControllerBase
    {
        private readonly IMenusService menuService = _menuService;

        [HttpGet("{roles}")]
        public IActionResult GetMenus(string roles)
        {
            if(roles == null)
                return Unauthorized();

            var menus = menuService.GetMenusByRole(roles.Split(","));

            if (menus == null)
                return NotFound();

            return Ok(menus);
        }
    }
}
