using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class AnswersRepository : IAnswersRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public AnswersRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }
        public async Task<AnswerDTO> Add(Answers answers)
        {
            var isusser = await _getUser.user();
            var an = new Answers
            {
                Contain = answers.Contain,
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
                UserName= isusser.UserName,
                Date = an.Date,
                Contain = an.Contain
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
        private List<AnswerDTO> CreateListAnswerDTO(List<Answers>? ans, ApplicationUser user)
        {
            if (ans == null || ans.Count == 0)
            {
                return new List<AnswerDTO>();
            }
            var answers = ans.Select(an => new AnswerDTO
            {
                Id = an.Id,
                Avatar = user.Avatar,
                UserName = user.UserName,
                Date = an.Date,
                Contain = an.Contain
            }).ToList();
            return answers;
        }
        public async Task<List<AnswerDTO>> GetAll()
        {
            var isusser = await _getUser.user();
            var result = await _context.Answers.ToListAsync();
            var a = CreateListAnswerDTO(result, isusser);
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
                UserName = isusser.UserName,
                Date = result.Date,
                Contain = result.Contain
            };
        }

        public async Task<List<AnswerDTO>> GetByQuestionId(int id)
        {
            var isusser = await _getUser.user();
            var result = await _context.Answers.Where(a => a.QuenstionId == id).ToListAsync();
            var a = CreateListAnswerDTO(result, isusser);
            return a;
        }

        public async Task<bool> Update(UpdateAnswerModel answer)
        {
            var result = await _context.Answers.SingleOrDefaultAsync(a => a.Id == answer.Id);
            if(result == null || answer.Contain == null || answer.Contain.Count() == 0) { return false;}
            result.Contain = answer.Contain;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
