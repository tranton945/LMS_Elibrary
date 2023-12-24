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
    [Authorize(Roles = "Teacher")]
    public class ClassRoomNotificationController : ControllerBase
    {
        private readonly BlacklistService _blacklist;
        private readonly IClassRoomNotificationRepository _classRoomNotification;

        public ClassRoomNotificationController(BlacklistService blacklist, IClassRoomNotificationRepository classRoomNotification) 
        {
            _blacklist = blacklist;
            _classRoomNotification = classRoomNotification;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateClassRoomNotificationModel Notification)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.Add(Notification);
                if(result == null)
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
                var result = await _classRoomNotification.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _classRoomNotification.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetClassRoomInPopUp")]
        public async Task<IActionResult> GetClassRoomInPopUp(int subjectId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.GetClassRoomInPopUp(subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _classRoomNotification.Search(searchString);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetClassRoomInScreenNotification")]
        public async Task<IActionResult> GetClassRoomInScreenNotification(int subjectId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.GetClassRoomInScreenNotification(subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetBySubjectId")]
        public async Task<IActionResult> GetBySubjectId(int subjectId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.GetBySubjectId(subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetByClassRoom")]
        public async Task<IActionResult> GetByClassRoom(string classRoomName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.GetByClassRoom(classRoomName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ClassRoomNotification Notification, int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.Update(Notification, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoomNotification.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
