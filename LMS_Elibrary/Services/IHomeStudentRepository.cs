using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IHomeStudentRepository
    {
        public Task<List<HomeStudentDTO>> GetAll();
        public Task<List<HomeStudentDTO>> Search(string searchString);
        public Task<List<HomeStudentDTO>> SortByName();
        public Task<List<HomeStudentDTO>> SortByStar(string type);
        public Task<Subject> GetBySubjectId(int subjectID);
        public Task<bool> LikeSubject(int subjectID);
        public Task<List<SubjectHomeTeacher>> DownloadResource(int subjectID);
        public Task<List<SubjectNotificationDTO>> Notification(int subjectID);
        public Task<List<QuestionDTO>> QuestionAndAnswer(int subjectID);
    }
}
