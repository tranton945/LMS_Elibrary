using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface ISubjectNotificationRepository
    {
        public Task<List<SubjectNotification>> GetAll();
        public Task<SubjectNotification> GetById(int id);
        public Task<List<SubjectNotification>> GetBySubjectId(int id);
        public Task<SubjectNotification> Add(SubjectNotification subjectNotification, int subjectId);
        public Task<bool> Update(SubjectNotification subjectNotification, int id);
        public Task<bool> Delete(int id);
    }
}
