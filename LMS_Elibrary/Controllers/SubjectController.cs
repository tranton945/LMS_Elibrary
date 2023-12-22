using LMS_Elibrary.Data;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly BlacklistService _blacklistService;
        private readonly ISubjectRepository _subject;
        private readonly IAccountRepository _account;

        public SubjectController(BlacklistService blacklistService, ISubjectRepository subject, IAccountRepository account) 
        {
            _blacklistService = blacklistService;
            _subject = subject;
            _account = account;
        }

        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.Add(subject);
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
                var result = await _subject.GetAll();
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
                var result = await _subject.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllSubjectName")]
        public async Task<IActionResult> GetAllSubjectName()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetAllSubjectName();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetSubjectByName")]
        public async Task<IActionResult> GetSubjectByName(string name)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetSubjectByName(name);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                //var listAccount = await _account.GetAllAccountRole();
                var result = await _subject.GetAllTeacher();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetSubjectByTeacher")]
        public async Task<IActionResult> GetSubjectByTeacher(string teacher)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetSubjectByTeacher(teacher);
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
                var result = await _subject.Search(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetSchoolYear")]
        public async Task<IActionResult> GetSchoolYear()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetSchoolYear();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetSubjectBySchoolYear")]
        public async Task<IActionResult> GetSubjectBySchoolYear(string schoolYear)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.GetSubjectBySchoolYear(schoolYear);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Subject subject, int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.Update(subject, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("AddTeacherToSubject")]
        public async Task<IActionResult> AddTeacherToSubject(int subjectId, string teacherName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.AddTeacherToSubject(subjectId, teacherName);
                if(result == false)
                {
                    return NotFound();
                }
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
                var result = await _subject.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
