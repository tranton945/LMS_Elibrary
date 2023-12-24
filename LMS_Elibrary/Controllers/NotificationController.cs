using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notification;
        private readonly BlacklistService _blacklist;

        public NotificationController(INotificationRepository notification, BlacklistService blacklist) 
        {
            _notification = notification;
            _blacklist = blacklist;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(Notification notification)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.Add(notification);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.GetAll();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchString)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.Search(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("Sort")]
        public async Task<IActionResult> Sort(string type)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.Sort(type);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("IsRead")]
        public async Task<IActionResult> IsRead(List<int> id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.IsRead(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(List<int> id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _notification.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
