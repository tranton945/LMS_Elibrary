using LMS_Elibrary.Data;
using LMS_Elibrary.Migrations;
using LMS_Elibrary.Models;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectEditController : ControllerBase
    {
        private readonly BlacklistService _blacklist;
        private readonly ISubjectRoleTeacherRepository _subjectRoleTeacher;
        private readonly ITopicRepository _topic;
        private readonly ILectureRepository _lecture;
        private readonly IClassRoomRepository _classRoom;
        private readonly IDocumentRoleTeacherRepository _documentRoleTeacher;

        public SubjectEditController(IDocumentRoleTeacherRepository documentRoleTeacher,IClassRoomRepository classRoom ,ILectureRepository lecture,BlacklistService blacklist, ISubjectRoleTeacherRepository subjectRoleTeacher, ITopicRepository topic) 
        {
            _blacklist = blacklist;
            _subjectRoleTeacher = subjectRoleTeacher;
            _topic = topic;
            _lecture = lecture;
            _classRoom = classRoom;
            _documentRoleTeacher = documentRoleTeacher;
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
        [HttpPut("UpdateSubjectDescriptions")]
        public async Task<IActionResult> UpdateSubjectDescriptions(int subId, string content)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectRoleTeacher.UpdateSubjectDescriptions(subId, content);
                //if (result == false)
                //{
                //    return BadRequest();
                //}
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("AddSubjectOtherInformation")]
        public async Task<IActionResult> AddSubjectOtherInformation(int subId, string title, string content)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectRoleTeacher.AddSubjectOtherInformation(subId, title, content);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("DeleteSubjectOtherInformation")]
        public async Task<IActionResult> DeleteSubjectOtherInformation(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _subjectRoleTeacher.DeleteSubjectOtherInformation(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("newTopic")]
        public async Task<IActionResult> Add(CreateTopicModel topic)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
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
        [HttpGet("GetAllTopic")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
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
        [HttpDelete("DeleteTopic")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
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
        [HttpPost("newLecture")]
        public async Task<IActionResult> newLecture(CreateLectureModel lecture)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.Add(lecture);
                if(result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetLectureById")]
        public async Task<IActionResult> GetLectureById(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.GetById(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateLecture")]
        public async Task<IActionResult> UpdateLecture(int id, CreateLectureModel lecture)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.Update(lecture, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("DeleteLecture")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("BlockStudent")]
        public async Task<IActionResult> BlockStudent(int id, bool blockStudent)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.BlockStudent(id, blockStudent);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("AddLectureAndDocument")]
        public async Task<IActionResult> AddLectureAndDocument(LectureAndDocumentInput lectureAndDocumentInput)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.AddLectureAndDocument(lectureAndDocumentInput);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("SearchUploadDocument")]
        public async Task<IActionResult> SearchUploadDocument(string searchString, string type)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.SearchUploadDocument(searchString, type);
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
                if (await _blacklist.CheckJWT() == true)
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
        [HttpGet("AssignDocument")]
        public async Task<IActionResult> AssignDocument(int LectureId, List<string> classRooms)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _lecture.AssignDocument(LectureId, classRooms);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
