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
        private readonly ISubjectRoleTeacherRepository _subject;
        private readonly IClassRoomRepository _classRoom;

        public SubjectRoleTeacherController(IClassRoomRepository classRoom,BlacklistService blacklistService, ISubjectRoleTeacherRepository subject)
        {
            _blacklistService = blacklistService;
            _subject = subject;
            _classRoom = classRoom;
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
        [HttpGet("SubjectOverview")]
        public async Task<IActionResult> SubjectOverview(int subId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.SubjectOverview(subId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("ListTopicOverView")]
        public async Task<IActionResult> ListTopic(int subId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.ListTopic(subId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetClassRoomName")]
        public async Task<IActionResult> GetClassRoomName(int subjectId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoom.GetClassRoomBySubjectId(subjectId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("ListTopicAssignDocument")]
        public async Task<IActionResult> ListTopicAssignDocument(int subId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.ListTopicAssignDocument(subId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("ListLectureAssignDocument")]
        public async Task<IActionResult> ListLectureAssignDocument(string topicName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.ListLectureAssignDocument(topicName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("AssignDocument")]
        public async Task<IActionResult> AssignDocument(string lecture, List<string> classRooms)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subject.AssignDocument(lecture, classRooms);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
