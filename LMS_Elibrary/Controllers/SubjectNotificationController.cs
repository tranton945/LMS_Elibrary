using LMS_Elibrary.Data;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher, Admin")]
    public class SubjectNotificationController : ControllerBase
    {
        private readonly BlacklistService _blacklist;
        private readonly ISubjectNotificationRepository _subjectNotification;

        public SubjectNotificationController(BlacklistService blacklist, ISubjectNotificationRepository subjectNotification) 
        {
            _blacklist = blacklist;
            _subjectNotification = subjectNotification;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(SubjectNotification subjectNotification, int subjectId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectNotification.Add(subjectNotification, subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetBySubjectId")]
        public async Task<IActionResult> GetBySubjectId(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectNotification.GetBySubjectId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
