using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MCAnswerController : ControllerBase
    {
        private readonly IMCAnswerRepository _mCAnswer;
        private readonly BlacklistService _blacklist;

        public MCAnswerController(IMCAnswerRepository mCAnswer, BlacklistService blacklist) 
        {
            _mCAnswer = mCAnswer;
            _blacklist = blacklist;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(List<MCAnswers> MCQuestion, int MCQuestionId)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCAnswer.Add(MCQuestion, MCQuestionId);
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
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCAnswer.GetAll();
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
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCAnswer.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetByMCQuestion")]
        public async Task<IActionResult> GetByMCQuestion(int id)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCAnswer.GetByMCQuestion(id);
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
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCAnswer.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
