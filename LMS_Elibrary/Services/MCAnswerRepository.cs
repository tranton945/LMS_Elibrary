using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class MCAnswerRepository : IMCAnswerRepository
    {
        private readonly ElibraryDbContext _context;

        public MCAnswerRepository(ElibraryDbContext context) 
        {
            _context = context;
        }
        public async Task<bool> Add(List<MCAnswers> MCAnswers, int MCQuestionId)
        {
            var quest = await _context.MCQuestions.SingleOrDefaultAsync(a => a.Id == MCQuestionId);
            if (quest == null)
            {
                return false;
            }
            foreach (var newAnswer in MCAnswers)
            {
                var MCA = new MCAnswers
                {
                    Content = newAnswer.Content,
                    IsCorrect = newAnswer.IsCorrect,
                };
                _context.MCAnswers.Add(MCA);
                await _context.SaveChangesAsync();

                var newMapping = new QuestionAnswerMapping
                {
                    MCQuestionId = quest.Id,
                    MCAnswerId = MCA.id
                };
                _context.QuestionAnswerMapping.Add(newMapping);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var answerToDelete = await _context.Answers.SingleOrDefaultAsync(a => a.Id == id);
            if (answerToDelete != null)
            {
                // Xóa các liên kết trong bảng QuestionAnswerMapping
                var mappingsToDelete = await _context.QuestionAnswerMapping
                    .Where(qam => qam.MCAnswerId == id).ToListAsync();

                _context.QuestionAnswerMapping.RemoveRange(mappingsToDelete);


                // xóa đáp án
                _context.Answers.Remove(answerToDelete);

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                return true;
            }
            return false;

        }

        public async Task<List<MCAnswers>> GetAll()
        {
            var result = await _context.MCAnswers.ToListAsync();
            return result;
        }

        public Task<MCAnswers> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MCAnswers>> GetByMCQuestion(int id)
        {
            var answers = await _context.QuestionAnswerMapping
                 .Where(qam => qam.MCQuestionId == id)
                 .Select(qam => qam.MCAnswers)
                 .ToListAsync();
            return answers;
        }

        public Task<bool> Update(MCAnswers MCQuestion, int id)
        {
            throw new NotImplementedException();
        }
    }
}
