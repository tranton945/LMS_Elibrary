using LMS_Elibrary.Data;
using LMS_Elibrary.Helper;
using LMS_Elibrary.Migrations;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManageLessioneRoleTeacherController : ControllerBase
    {
        private readonly BlacklistService _blacklist;
        private readonly IManageLessionRepository _manageLession;
        private readonly IClassRoomRepository _classRoom;

        public ManageLessioneRoleTeacherController(IClassRoomRepository classRoom,BlacklistService blacklist, IManageLessionRepository manageLession)
        {
            _blacklist = blacklist;
            _manageLession = manageLession;
            _classRoom = classRoom;
        }
        [HttpGet("ManageGetAllLession")]
        public async Task<IActionResult> ManageGetAllLession()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageGetAllLession();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageGetSubjectName")]
        public async Task<IActionResult> ManageGetSubjectName()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageGetSubjectName();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageGetSubjectNamePopup")]
        public async Task<IActionResult> ManageGetSubjectNamePopup()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageGetSubjectNamePopup();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageGetBySubjectName")]
        public async Task<IActionResult> ManageGetBySubjectName(string SubjectName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageGetBySubjectName(SubjectName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageSearch")]
        public async Task<IActionResult> ManageSearch(string searchString)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageSearch(searchString);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("ManageUpload")]
        public async Task<IActionResult> ManageUpload([FromForm] List<IFormFile> files, string subjectName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageUpload(files, subjectName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageDownload")]
        public async Task<IActionResult> ManageDownload(List<int> DocIds)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageDownload(DocIds);
                if (result == null || result.Count() == 0)
                {
                    return BadRequest("Document not found or empty.");
                }
                var zipBytes = ZipHelper.CreateZipFileFromDocuments(result, $"{DateTime.Now}.zip");
                return File(zipBytes, "application/zip", $"{DateTime.Now}.zip");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManagePreview")]
        public async Task<IActionResult> ManagePreview(int docId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManagePreview(docId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut("ManageChangeName")]
        public async Task<IActionResult> ManageChangeName(int docId, string newName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageChangeName(docId, newName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageGetTopicBySubjectName")]
        public async Task<IActionResult> ManageGetTopicBySubjectName(string subjectName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageGetTopicBySubjectName(subjectName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetClassRoomName")]
        public async Task<IActionResult> GetClassRoomName(string subName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _classRoom.GetClassRoomBySubjectName(subName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("ManageAddToSubject")]
        public async Task<IActionResult> ManageAddToSubject(int docId, string topic, string lectureName, List<string>? classRoomNames)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageAddToSubject(docId, topic, lectureName, classRoomNames);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("ManageDeleteDoc")]
        public async Task<IActionResult> ManageDeleteDoc(List<int> DocIds)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageLession.ManageDeleteDoc(DocIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
