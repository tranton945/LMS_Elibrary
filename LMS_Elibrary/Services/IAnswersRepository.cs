using LMS_Elibrary.Data;
using LMS_Elibrary.Migrations;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IAnswersRepository
    {
        public Task<List<AnswerDTO>> GetAll();
        public Task<AnswerDTO> GetById(int id);
        public Task<List<AnswerDTO>> GetByQuestionId(int id);
        public Task<AnswerDTO> Add(Answers answers);
        public Task<bool> Update(UpdateAnswerModel answer);
        public Task<bool> Delete(int id);
    }
}
