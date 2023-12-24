using LMS_Elibrary.Data;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher, Admin, Leadership")]
    public class ClassRoomController : ControllerBase
    {
        private readonly IClassRoomRepository _classRoom;
        private readonly BlacklistService _blacklist;

        public ClassRoomController(IClassRoomRepository classRoom, BlacklistService blacklist) 
        {
            _classRoom = classRoom;
            _blacklist = blacklist;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(ClassRoom classRoom)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoom.Add(classRoom);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _classRoom.GetAll();
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
                var result = await _classRoom.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Update")]
        public async Task<IActionResult> Update(ClassRoom classRoom, int subId, int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoom.Update(classRoom, subId, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoom.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }        
    }
}
