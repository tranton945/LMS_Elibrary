using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IQuestionRepository
    {
        public Task<List<QuestionDTO>> GetAll();
        public Task<List<string>> GetAllTopic();
        public Task<List<string>> GetAllClassRoom();
        public Task<List<string>> GetLectureByTopic(string topic);
        public Task<QuestionDTO> GetById(int id);
        public Task<List<QuestionDTO>> GetByClassRoom(string classRoom);
        public Task<List<QuestionDTO>> GetByLecture(string lecture);
        public Task<List<string>> GetQuestionType();
        public Task<List<QuestionDTO>> GetQuestionByType(string type);
        public Task<List<string>> GetQuestionFilter();
        public Task<List<QuestionDTO>> GetQuestionByFilter(string filter);
        public Task<List<string>> GetAllLecture();
        public Task<List<QuestionDTO>> GetByDate();
        public Task<List<QuestionDTO>> GetByAnswered(bool answered);
        public Task<List<QuestionDTO>> GetMyQuestions();
        public Task<bool> LikeQuestion(int questionId);
        public Task<List<QuestionDTO>> GetMyLike();
        public Task<bool> HidentAnswer(int questionId, bool hidentAnswer);
        public Task<Questions> Add(CreateQuestionModel q);
        public Task<bool> Update(Questions q, int id);
        public Task<bool> Delete(int id);
    }
}
