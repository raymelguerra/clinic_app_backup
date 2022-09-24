using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ClinicApp.AuthorizationAndUserManager.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.AuthorizationAndUserManager.Models;

namespace ClinicApp.AuthorizationAndUserManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private IUserService _userService;
    private readonly clinicbdContext _context;
    public AuthController(clinicbdContext context, IUserService userService)
    {
        _userService = userService;
        _context = context;
    }

    // /api/auth/register
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUserAsync(model);

            if (result.IsSuccess)
                return Ok(result); // Status Code: 200 

            return BadRequest(result);
        }

        return BadRequest("Some properties are not valid"); // Status code: 400
    }

    // /api/auth/login
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.LoginUserAsync(model);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        return BadRequest("Some properties are not valid");
    }

    [HttpGet(Name ="GET All")]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> Get() 
    {
        var result = await _userService.GetAllUsersAsync(); ;
        return Ok(result);
    }

   [HttpPost("ResetPassword"), Authorize(Roles = "Administrator,Operator,Biller")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.ResetPasswordAsync(model);

            if (result.IsSuccess)
                return Ok(result);

            return StatusCode(409, result);
        }

        return BadRequest("Some properties are not valid");
    }

    [HttpGet("roles", Name = "Get All Roles")]
    public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
    {
        var list = await _context.Roles.OrderBy(o => o.Name)
            .ToListAsync();
        return Ok(list);
    }

    [HttpDelete("{id}", Name = "Delete user"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userService.DeleteUserAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
