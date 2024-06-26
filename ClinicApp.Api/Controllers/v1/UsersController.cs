using Asp.Versioning;
using ClinicApp.Core.Dtos;
using ClinicApp.Infrastructure.Dto;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Options;
using Oauth2.sdk.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicApp.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class UsersController(
        IUsersService _usersService,
        IOptions<CredentialsSettings> _options
        ) : ControllerBase
    {
        private readonly IUsersService usersService = _usersService;
        private readonly CredentialsSettings credentials = _options.Value;

        /// <summary>
        /// Get Users list.
        /// </summary>
        /// <response code="200">Returns Users list.</response>
        /// <response code="401">Invalid authentication.</response>
        /// <response code="403">Invalid authorization.</response>
        /// <response code="404">Users not found.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await usersService.GetAllUsers();
            if (userList.Any())
                return Ok(userList);

            return NotFound();
        }

        /// <summary>
        /// Get User.
        /// </summary>
        /// <response code="200">Returns User.</response>
        /// <response code="401">Invalid authentication.</response>
        /// <response code="403">Invalid authorization.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await usersService.GetUser(userId);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Create User.
        /// </summary>
        /// <response code="201">User Created.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Invalid authentication.</response>
        /// <response code="403">Invalid authorization.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserVM user)
        {
            var created = await usersService.CreateUser(user);
            if (created)
                return Created();

            return BadRequest();
        }

        /// <summary>
        /// Update User.
        /// </summary>
        /// <response code="200">User Updated.</response>
        /// <response code="400">Bad Request.</response>
        /// <response code="401">Invalid authentication.</response>
        /// <response code="403">Invalid authorization.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [HttpPut("{userId}")]
        [EnableQuery]
        public async Task<IActionResult> UpdateUser(
            string userId,
            [FromBody] UserVM user
        )
        {
            bool updated;
            updated = await usersService.UpdateUser(userId, user);
            if (updated)
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Delete User.
        /// </summary>
        /// <response code="200">User Deleted.</response>
        /// <response code="401">Invalid authentication.</response>
        /// <response code="403">Invalid authorization.</response>
        /// <response code="404">User Not found.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(
            string userId
        )
        {
            var deleted = await usersService.DeleteUser(userId);
            if (deleted)
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Change Password.
        /// </summary>
        /// <response code="200">Password changed.</response>
        /// <response code="401">Invalid authentication.</response>
        /// <response code="403">Invalid authorization.</response>
        /// <response code="404">User Not found.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPut("{userId}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordDto pass)
        {
            var changed = await usersService.ChangePassword(userId, pass);
            if (changed)
                return Ok();

            return BadRequest();
        }
    }
}
