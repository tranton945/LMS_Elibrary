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
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _file;
        private readonly BlacklistService _blacklistService;

        public FileController(IFileRepository file, BlacklistService blacklistService) 
        {
            _file = file;
            _blacklistService = blacklistService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] IFormFile file, int docId)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _file.Add(file, docId);
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
                var result = await _file.GetAll();
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
                var result = await _file.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(IFormFile file, int docId, int id)
        {
            try
            {
                if (await _blacklistService.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _file.Update(file, docId, id);
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
                var result = await _file.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
