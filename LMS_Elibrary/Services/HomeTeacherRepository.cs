using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class HomeTeacherRepository : IHomeTeacherRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public HomeTeacherRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }

        public async Task<List<Notification>> Notification()
        {
            var result = await _context.Notifications.OrderByDescending(a => a.Id).ToListAsync();
            return result.Take(6).ToList();
        }

        public async Task<List<SubjectHomeTeacher>> Subject()
        {
            var isuser = await _getUser.user();
            var aq = await _context.Teachers.Where(a => a.UserId == isuser.Id).Select(a => a.SubjectId).ToListAsync();
            var sub = await _context.Subjects.Where(a => aq.Contains(a.Id)).ToListAsync();
            var sub10 = sub.Take(10).ToList();
            var result = new List<SubjectHomeTeacher>();
            foreach (var item in sub10)
            {
                var q = new SubjectHomeTeacher
                {
                    subjectName = item.SubjectName,
                    subjectKey = item.SubjectId,
                };
                result.Add(q);
            }
            return result;
        }

        public async Task<int> TotalExam()
        {
            var a = await _context.Exams.ToListAsync();
            return a.Count;
        }

        public async Task<int> TotalLession()
        {
            var a = await _context.Documents.Where(a => a.Type == "Bài giảng").ToListAsync();
            return a.Count;
        }

        public async Task<int> TotalResoucre()
        {
            var a = await _context.Documents.Where(a => a.Type == "Tài nguyên").ToListAsync();
            return a.Count;
        }

        public async Task<int> TotalSubject()
        {
            var isuser = await _getUser.user();
            var aq = await _context.Teachers.Where(a => a.UserId == isuser.Id).Select(a => a.SubjectId).ToListAsync();
            var sub = await _context.Subjects.Where(a => aq.Contains(a.Id)).ToListAsync();
            return sub.Count;
        }
    }
}
