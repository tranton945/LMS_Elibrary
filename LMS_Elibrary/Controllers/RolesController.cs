using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoles _roles;
        private readonly BlacklistService _blacklistService;

        public RolesController(IRoles roles, BlacklistService blacklistService)
        {
            _roles = roles;
            _blacklistService = blacklistService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _roles.GetAllRoles();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetRole(string role)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var _role = await _roles.GetByName(role);
                return Ok(_role);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("AddNewRoll")]
        public async Task<IActionResult> AddNewRoll(string role)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _roles.AddRole(role);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                return Unauthorized();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string role)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                await _roles.DeleteRole(role);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("assign-UserRole")]
        public async Task<IActionResult> AddUserRole(string email, string roleName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _roles.AddUserRole(email, roleName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(string email, string newRoleName, string oldRoleName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _roles.ChangeUserRole(email, newRoleName, oldRoleName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("remove-UserRole")]
        public async Task<IActionResult> RemoveUserRole(string email, string roleName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _roles.RemoveUserRole(email, roleName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
