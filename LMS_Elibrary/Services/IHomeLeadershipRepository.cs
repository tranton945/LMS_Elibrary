using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IHomeLeadershipRepository
    {
        public Task<int> TotalSubject();
        public Task<int> TotalTeacher();
        public Task<int> TotalPrivateFile();
        public Task<int> TotalExam();
        public Task<List<SubjectHomeLeadership>> SubjectAccessHistory();
        public Task<List<string>> PrivateFile();
    }
}
