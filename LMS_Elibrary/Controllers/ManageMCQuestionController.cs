using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using LMS_Elibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageMCQuestionController : ControllerBase
    {
        private readonly BlacklistService _blacklist;
        private readonly IMCAnswerRepository _mCAnswer;
        private readonly IMCQuestionRepository _mCQuestion;

        public ManageMCQuestionController(BlacklistService blacklist , IMCAnswerRepository mCAnswer, IMCQuestionRepository mCQuestion)
        {
            _blacklist = blacklist;
            _mCAnswer = mCAnswer;
            _mCQuestion = mCQuestion;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateMCQuestionModel MCQuestion)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.Add(MCQuestion);
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.GetAll();
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
                var result = await _mCQuestion.GetById(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, string questionContent, List<MCAnswers> mCAnswers)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.Update(id, questionContent, mCAnswers);
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
                var result = await _mCQuestion.Delete(id);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetSubjectGroup")]
        public async Task<IActionResult> GetSubjectGroup()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.GetSubjectGroup();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetSubject")]
        public async Task<IActionResult> GetSubject()
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.GetSubject();
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
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.Search(searchString);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("SortQuestion")]
        public async Task<IActionResult> SortQuestion(string subjectGroup, string subject, List<string>? levels)
        {
            try
            {
                if (await _blacklist.CheckJWT() == true)
                {
                    return BadRequest("access token invalid");
                }
                var result = await _mCQuestion.SortQuestion(subjectGroup, subject, levels);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
