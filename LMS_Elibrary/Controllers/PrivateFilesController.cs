using LMS_Elibrary.Data;
using LMS_Elibrary.Helper;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Leadership")]
    public class PrivateFilesController : ControllerBase
    {
        private readonly IPrivateFilesRepository _privateFiles;
        private readonly BlacklistService _blacklistService;

        public PrivateFilesController(IPrivateFilesRepository privateFiles, BlacklistService blacklistService) 
        {
            _privateFiles = privateFiles;
            _blacklistService = blacklistService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] List<IFormFile> files)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _privateFiles.Add(files);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _privateFiles.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _privateFiles.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("GetType")]
        public async Task<IActionResult> GetAllType()
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _privateFiles.GetAllFileType();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchStr)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _privateFiles.Search(searchStr);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetPrivateFileByType")]
        public async Task<IActionResult> GetPrivateFileByType(List<string> types)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _privateFiles.GetPrivateFileByType(types);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("ChangeFileName")]
        public async Task<IActionResult> ChangeFileName(string name, int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _privateFiles.ChangeFileName(name, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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
                var result = await _privateFiles.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(List<int> listDocId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _privateFiles.DownloadFile(listDocId);
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
    }
}
