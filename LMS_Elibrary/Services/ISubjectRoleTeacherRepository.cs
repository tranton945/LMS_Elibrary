using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface ISubjectRoleTeacherRepository
    {
        public Task<List<SubjectTeacherDTO>> GetAllRoleTeacher();
        public Task<List<SubjectTeacherDTO>> SearchTeacher(string searchString);
        public Task<List<string>> GetFilterTeacherString();
        public Task<List<SubjectTeacherDTO>> GetSubjectByNameRoleTeacher(string type);
        public Task<Subject> SubjectOverview(int subId);
        public Task<List<Topic>> ListTopic(int subId);
        public Task<List<SubjectTeacherDTO>> QuestAndAnswer(string type);
        public Task<List<SubjectTeacherDTO>> SubjectNotifiaction(string type);
    }
}
