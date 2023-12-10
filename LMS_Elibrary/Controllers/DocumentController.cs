using LMS_Elibrary.Data;
using LMS_Elibrary.Helper;
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
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _document;
        private readonly BlacklistService _blacklistService;

        public DocumentController(IDocumentRepository document, BlacklistService blacklistService) 
        {
            _document = document;
            _blacklistService = blacklistService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] Document document, IFormFile file)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.Add(document, file);
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
                var result = await _document.GetAll();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocInforById")]
        public async Task<IActionResult> GetDocInforById(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetDocInforById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllDocInfor")]
        public async Task<IActionResult> GetAllDocInfor()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetAllDocInfor();
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
                var result = await _document.GetById(id);
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
                var result = await _document.Search(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocByName")]
        public async Task<IActionResult> GetDocByName(string docName)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetDocByName(docName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetAllDocName")]
        public async Task<IActionResult> GetAllDocName()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetAllDocName();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocByTeacher")]
        public async Task<IActionResult> GetDocByTeacher(string teacher)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetDocByTeacher(teacher);
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
                var result = await _document.GetAllTeacher();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocByApprove")]
        public async Task<IActionResult> GetDocByApprove(string type)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetDocByApprove(type);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetApproveType")]
        public async Task<IActionResult> GetApproveType()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetApproveType();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetDocBySchoolYear")]
        public async Task<IActionResult> GetDocBySchoolYear(string schoolYear)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.GetDocBySchoolYear(schoolYear);
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
                var result = await _document.GetSchoolYear();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("DownloadDocFile")]
        public async Task<IActionResult> DownloadDocFile(List<int> listDocId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.DownloadDocFile(listDocId);
                if (result == null || result.Count() == 0)
                {
                    return BadRequest("Document not found or empty.");
                }
                var zipBytes = ZipHelper.CreateZipFileFromDocuments(result, $"{DateTime.Now}.zip");
                return File(zipBytes, "application/zip", $"{DateTime.Now}.zip");
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, Document document)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.Update(document, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("ApproveDoc")]
        public async Task<IActionResult> ApproveDoc(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.ApproveDoc(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("PopUpDoNotApproveDoc")]
        public async Task<IActionResult> PopUpDoNotApproveDoc(int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.PopUpDoNotApproveDoc(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("DoNotApproveDoc")]
        public async Task<IActionResult> DoNotApproveDoc(DoNotApproveDocument doNotApproveDocument)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _document.DoNotApproveDoc(doNotApproveDocument);
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
                var result = await _document.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
