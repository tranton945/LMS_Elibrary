using LMS_Elibrary.Data;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TopicController : ControllerBase
    {
        private readonly ITopicRepository _topic;
        private readonly BlacklistService _blacklistService;

        public TopicController(ITopicRepository topic, BlacklistService blacklist) 
        {
            _topic = topic;
            _blacklistService = blacklist;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Topic topic)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _topic.Add(topic);
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
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _topic.GetAll();
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
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _topic.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, Topic topic)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _topic.Update(topic, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _topic.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
