using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeTeacherController : ControllerBase
    {
        private readonly IHomeTeacherRepository _homeTeacher;
        private readonly BlacklistService _blacklist;

        public HomeTeacherController(IHomeTeacherRepository homeTeacher, BlacklistService blacklist) 
        {
            _homeTeacher = homeTeacher;
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
                var result = await _homeTeacher.TotalSubject();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("TotalLession")]
        public async Task<IActionResult> TotalLession()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeTeacher.TotalLession();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("TotalResoucre")]
        public async Task<IActionResult> TotalResoucre()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeTeacher.TotalResoucre();
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
                var result = await _homeTeacher.TotalExam();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("Subject")]
        public async Task<IActionResult> Subject()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeTeacher.Subject();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> Notification()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeTeacher.Notification();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
