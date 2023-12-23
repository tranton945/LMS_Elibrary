using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IMCAnswerRepository
    {
        public Task<List<MCAnswers>> GetAll();
        public Task<MCAnswers> GetById(int id);
        public Task<List<MCAnswers>> GetByMCQuestion(int id);
        public Task<bool> Add(List<MCAnswers> MCQuestion, int MCQuestionId);
        public Task<bool> Update(MCAnswers MCQuestion, int id);
        public Task<bool> Delete(int id);
    }
}
