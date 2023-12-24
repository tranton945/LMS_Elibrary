using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher, Admin")]
    public class DocumentRoleTeacherController : ControllerBase
    {
        private readonly IDocumentRoleTeacherRepository _documentRoleTeacher;
        private readonly BlacklistService _blacklistService;

        public DocumentRoleTeacherController(IDocumentRoleTeacherRepository documentRoleTeacher, BlacklistService blacklistService) 
        {
            _documentRoleTeacher = documentRoleTeacher;
            _blacklistService = blacklistService;
        }
        [HttpGet("PopupAddLecture")]
        public async Task<IActionResult> PopupAddLecture(int subId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.PopupAddLecture(subId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("AddLecture")]
        public async Task<IActionResult> AddLecture([FromForm] AddDocRoleTeacherModel addDocRoleTeacherModel, List<IFormFile> files)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.AddLecture(addDocRoleTeacherModel, files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("PopupAddResources")]
        public async Task<IActionResult> PopupAddResources(int subId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.PopupAddResources(subId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("AddResources")]
        public async Task<IActionResult> AddResources([FromForm] AddDocRoleTeacherModel addDocRoleTeacherModel, List<IFormFile> files)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.AddResources(addDocRoleTeacherModel, files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetAllBySubjectId")]
        public async Task<IActionResult> GetAllBySubjectId(int subId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.GetAllBySubjectId(subId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetDocByStatus")]
        public async Task<IActionResult> GetDocByStatus(string type)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.GetDocByStatus(type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetStatus")]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.GetStatus();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _documentRoleTeacher.Search(searchString);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("SortDocument")]
        public async Task<IActionResult> SortDocument(string columnName, bool isAscending)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.SortDocument(columnName, isAscending);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("RreviewPopup")]
        public async Task<IActionResult> RreviewPopup(int docId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.RreviewPopup(docId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(List<int> ints)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.DownloadDocFile(ints);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(List<int> id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _documentRoleTeacher.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
