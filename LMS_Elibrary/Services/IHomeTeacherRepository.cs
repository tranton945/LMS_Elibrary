using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IHomeTeacherRepository
    {
        public Task<int> TotalSubject();
        public Task<int> TotalLession();
        public Task<int> TotalResoucre();
        public Task<int> TotalExam();
        public Task<List<SubjectHomeTeacher>> Subject();
        public Task<List<Notification>> Notification();
    }
}
