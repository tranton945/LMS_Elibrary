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
    [Authorize(Roles = "Admin, Leadership")]
    public class StudentController : ControllerBase
    {
        private readonly IHomeStudentRepository _homeStudent;
        private readonly BlacklistService _blacklist;

        public StudentController(IHomeStudentRepository homeStudent, BlacklistService blacklist) 
        {
            _homeStudent = homeStudent;
            _blacklist = blacklist;
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
                var result = await _homeStudent.GetAll();
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
                var result = await _homeStudent.Search(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SortByName")]
        public async Task<IActionResult> SortByName()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeStudent.SortByName();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SortByStar")]
        public async Task<IActionResult> SortByStar(string type)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeStudent.SortByStar(type);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetBySubjectId")]
        public async Task<IActionResult> GetBySubjectId(int subjectID)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeStudent.GetBySubjectId(subjectID);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("LikeSubject")]
        public async Task<IActionResult> LikeSubject(int subjectID)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeStudent.LikeSubject(subjectID);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("QuestionAndAnswer")]
        public async Task<IActionResult> QuestionAndAnswer(int subjectID)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeStudent.QuestionAndAnswer(subjectID);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> Notification(int subjectID)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _homeStudent.Notification(subjectID);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
