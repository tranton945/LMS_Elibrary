using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectRoleTeacherController : ControllerBase
    {
        private readonly BlacklistService _blacklistService;
        private readonly ISubjectRepository _subject;
        private readonly IAccountRepository _account;

        public SubjectRoleTeacherController(BlacklistService blacklistService, ISubjectRepository subject, IAccountRepository account)
        {
            _blacklistService = blacklistService;
            _subject = subject;
            _account = account;
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
                var result = await _subject.GetAllRoleTeacher();
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
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.SearchTeacher(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetFilter")]
        public async Task<IActionResult> GetFilter()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetFilterTeacherString();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetSubjectByFilter")]
        public async Task<IActionResult> GetSFilter(string type)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetSubjectByNameRoleTeacher(type);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
