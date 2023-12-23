using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IMCQuestionRepository
    {
        public Task<List<MCQuestionsDTO>> GetAll();
        public Task<List<MCQuestionsDTO>> Search(string searchString);
        public Task<List<string>> GetSubjectGroup();
        public Task<List<string>> GetSubject();
        public Task<MCQuestionWithMCAnswersDTO> GetById(int id);
        public Task<List<MCQuestionsDTO>> SortQuestion(string subjectGroup, string subject, List<string>? levels);
        public Task<MCQuestionsDTO> Add(CreateMCQuestionModel MCQuestion);
        public Task<bool> Update(int id, string questionContent, List<MCAnswers> mCAnswers);
        public Task<bool> Delete(int id);
        public Task<bool> SortQuestion(string subjectGroup, string subject, IFormFile formFile);
    }
}
