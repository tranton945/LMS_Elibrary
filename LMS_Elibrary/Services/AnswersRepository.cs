using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class AnswersRepository : IAnswersRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersRepository(ElibraryDbContext context, GetUser getUser, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _getUser = getUser;
            _userManager = userManager;
        }
        public async Task<AnswerDTO> Add(Answers answers)
        {
            var isusser = await _getUser.user();
            var an = new Answers
            {
                Content = answers.Content,
                QuenstionId= answers.QuenstionId,
                UserId = isusser.Id,
                Date = DateTime.Now,
            };
            _context.Add(an);
            await _context.SaveChangesAsync();
            return new AnswerDTO
            {
                Id = an.Id,
                Avatar = isusser.Avatar,
                UserName= isusser.Name,
                Date = an.Date,
                Content = an.Content
            };
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Answers.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null) { return false; }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<List<AnswerDTO>> CreateListAnswerDTO(List<Answers>? ans)
        {
            if (ans == null || ans.Count == 0)
            {
                return new List<AnswerDTO>();
            }
            List<AnswerDTO> answerDTOs = new List<AnswerDTO>();
            foreach (var answer in ans)
            {
                var user = await _userManager.FindByIdAsync(answer.UserId);
                var _answer = new AnswerDTO
                {
                    Id = answer.Id,
                    Avatar = user != null ? user.Avatar : string.Empty,
                    UserName = user != null ? user.Name : string.Empty,
                    Date = answer.Date,
                    Content = answer.Content
                };
                answerDTOs.Add(_answer);
            }
            return answerDTOs;
        }
        public async Task<List<AnswerDTO>> GetAll()
        {
            var isusser = await _getUser.user();
            var result = await _context.Answers.ToListAsync();
            var a = await CreateListAnswerDTO(result);
            return a;
        }

        public async Task<AnswerDTO> GetById(int id)
        {
            var isusser = await _getUser.user();
            var result = await _context.Answers.SingleOrDefaultAsync(a => a.Id == id);
            if(result == null)
            {
                return new AnswerDTO();
            }
            return new AnswerDTO
            {
                Id = result.Id,
                Avatar = isusser.Avatar,
                UserName = isusser.Name,
                Date = result.Date,
                Content = result.Content
            };
        }

        public async Task<List<AnswerDTO>> GetByQuestionId(int id)
        {
            var isusser = await _getUser.user();
            var result = await _context.Answers.Where(a => a.QuenstionId == id).ToListAsync();
            var a = await CreateListAnswerDTO(result);
            return a;
        }

        public async Task<bool> Update(UpdateAnswerModel answer)
        {
            var result = await _context.Answers.SingleOrDefaultAsync(a => a.Id == answer.Id);
            if(result == null || answer.Content == null || answer.Content.Count() == 0) { return false;}
            result.Content = answer.Content;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
