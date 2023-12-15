﻿using LMS_Elibrary.Models;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _account;
        private readonly BlacklistService _blacklistService;

        public AccountsController(IAccountRepository account, BlacklistService blacklistService)
        {
            _account = account;
            _blacklistService = blacklistService;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            var result = await _account.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return Unauthorized();
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _account.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpPost("SignOut")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _account.SignOut();
            return Ok(result);
        }

        [HttpGet("GetAllAccounts")]
        [Authorize(Roles = "Admin")]
        //[Authorize]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.GetAllAccounts();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("SelectAccountByEmail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccountByEmail(string email)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.GetAccountsByEmail(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateAccount")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> UpdateAccount(string email, string newName, DateTime newDateOfBirt, string newGender, string newPhoneNumber, string newTeacherID, string newAddress)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.UpdateAccount(email, newName, newDateOfBirt, newGender, newPhoneNumber, newTeacherID, newAddress);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateAvatar")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar(IFormFile fileAvatar)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.UpdateAvatar(fileAvatar);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdatePassword")]
        //[Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(ChangePasswordModel model)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.UpdatePassword(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteAccount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(string email)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.DeleteAccount(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SelectAccountRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SelectAccountRole(string email)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _account.GetAccountRole(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
