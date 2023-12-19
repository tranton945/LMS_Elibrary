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
    public class OverviewPriviewTeacherController : ControllerBase
    {
        private readonly BlacklistService _blacklist;
        private readonly ISubjectRoleTeacherRepository _subjectRoleTeacher;
        private readonly IClassRoomNotificationRepository _classRoomNotification;
        private readonly IQuestionRepository _question;

        public OverviewPriviewTeacherController(IQuestionRepository question, BlacklistService blacklist, ISubjectRoleTeacherRepository subjectRoleTeacher, IClassRoomNotificationRepository classRoomNotification) 
        {
            _blacklist = blacklist;
            _subjectRoleTeacher = subjectRoleTeacher;
            _classRoomNotification = classRoomNotification;
            _question = question;
        }
        [HttpGet("GetOverview")]
        public async Task<IActionResult> SubjectOverviewPriview(int subId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectRoleTeacher.SubjectOverviewPriview(subId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetQuestionAndAnwser")]
        public async Task<IActionResult> QuestionAndAnwser()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _question.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetClassRoomNotification")]
        public async Task<IActionResult> GetClassRoomNotification(int subjectId)
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
        [HttpGet("GetTopicAndLecture")]
        public async Task<IActionResult> GetTopicAndLecture(int subId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectRoleTeacher.ListTopic(subId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SubjectOverviewSearch(int subId, string searchString)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectRoleTeacher.SubjectOverviewSearch(subId, searchString);
                if(result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
