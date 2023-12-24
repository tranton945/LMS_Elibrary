using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin, Leadership")]
    public class HomeLeadershipController : ControllerBase
    {
        private readonly IHomeLeadershipRepository _homeLeadership;
        private readonly BlacklistService _blacklist;

        public HomeLeadershipController(IHomeLeadershipRepository homeLeadership, BlacklistService blacklist) 
        {
            _homeLeadership = homeLeadership;
            _blacklist = blacklist;
        }
        [HttpGet("TotalSubject")]
        public async Task<IActionResult> TotalSubject()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeLeadership.TotalSubject();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("TotalTeacher")]
        public async Task<IActionResult> TotalTeacher()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeLeadership.TotalTeacher();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("TotalPrivateFile")]
        public async Task<IActionResult> TotalPrivateFile()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeLeadership.TotalPrivateFile();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("TotalExam")]
        public async Task<IActionResult> TotalExam()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeLeadership.TotalExam();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SubjectAccessHistory")]
        public async Task<IActionResult> SubjectAccessHistory()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeLeadership.SubjectAccessHistory();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("PrivateFile")]
        public async Task<IActionResult> PrivateFile()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeLeadership.PrivateFile();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
