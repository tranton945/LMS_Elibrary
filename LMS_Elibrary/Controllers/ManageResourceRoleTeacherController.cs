using LMS_Elibrary.Data;
using LMS_Elibrary.Helper;
using LMS_Elibrary.Migrations;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageResourceRoleTeacherController : ControllerBase
    {
        private readonly ElibraryDbContext _context;
        private readonly IManageResourceRepository _manageResource;
        private readonly BlacklistService _blacklist;
        private readonly IClassRoomRepository _classRoom;

        public ManageResourceRoleTeacherController(IClassRoomRepository classRoom, BlacklistService blacklist,ElibraryDbContext context, IManageResourceRepository manageResource)
        {
            _context = context;
            _manageResource = manageResource;
            _blacklist = blacklist;
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
                var result = await _manageResource.ManageGetAllResource();
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
                var result = await _manageResource.ManageGetSubjectName();
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
                var result = await _manageResource.ManageGetSubjectNamePopup();
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
                var result = await _manageResource.ManageGetBySubjectName(SubjectName);
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
                var result = await _manageResource.ManageSearch(searchString);
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
                var result = await _manageResource.ManageUpload(files, subjectName);
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
                var result = await _manageResource.ManageDownload(DocIds);
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
                var result = await _manageResource.ManagePreview(docId);
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
                var result = await _manageResource.ManageChangeName(docId, newName);
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
        [HttpGet("ManageGetTopicBySubjectName")]
        public async Task<IActionResult> ManageGetTopicBySubjectName(string subjectName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageResource.ManageGetTopicBySubjectName(subjectName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ManageGetLectureByTopicName")]
        public async Task<IActionResult> ManageGetLectureByTopicName(string topicName)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageResource.ManageGetLectureByTopicName(topicName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("ManageAddToSubject")]
        public async Task<IActionResult> ManageAddToSubject(string lectureName, List<string>? classRoomNames)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _manageResource.ManageAddToSubject(lectureName, classRoomNames);
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
                var result = await _manageResource.ManageDeleteFile(DocIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
