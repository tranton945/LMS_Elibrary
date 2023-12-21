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
        public Task<Subject> SubjectOverviewPriview(int subId);
        public Task<bool> UpdateSubjectDescriptions(int subId, string content);
        public Task<SubjectOtherInformation> AddSubjectOtherInformation(int subId, string title, string content);
        public Task<bool> DeleteSubjectOtherInformation(int id);

        public Task<object> SubjectOverviewSearch(int subId, string searchString);
        public Task<List<string>> ListTopicAssignDocument(int subId);
        public Task<List<string>> ListLectureAssignDocument(string topicName);
        public Task<bool> AssignDocument(string lecture, List<string> classRooms);


    }
}
