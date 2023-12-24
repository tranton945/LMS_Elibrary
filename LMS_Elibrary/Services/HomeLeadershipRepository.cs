using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class HomeLeadershipRepository : IHomeLeadershipRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GetUser _getUser;

        public HomeLeadershipRepository(GetUser getUser,ElibraryDbContext context, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
            _getUser = getUser;
        }

        public async Task<List<string>> PrivateFile()
        {
            var a = await _context.PrivateFiles.Include(a => a.File).Select(a => a.File.FileName).ToListAsync();

            return a.Take(10).ToList();
        }

        public async Task<List<SubjectHomeLeadership>> SubjectAccessHistory()
        {
            var isuser = await _getUser.user();
            var s = await _context.SubAccessHistories.Where(a => a.UserId == isuser.Id).Select(a => a.SubjectId).ToListAsync();
            var sub = await _context.Subjects.Where(a => s.Contains(a.Id)).ToListAsync();
            var sub10 = sub.Take(10).ToList();
            var result = new List<SubjectHomeLeadership>();
            foreach(var item in sub10)
            {
                var q = new SubjectHomeLeadership
                {
                    subjectName = item.SubjectName,
                    subjectKey = item.SubjectId,
                    Teacher = item.Teacher
                };
                result.Add(q);
            }
            return result;
        }

        public async Task<int> TotalExam()
        {
            var  a = await _context.Exams.ToListAsync();
            return a.Count;
        }

        public async Task<int> TotalPrivateFile()
        {
            var a = await _context.PrivateFiles.ToListAsync();
            return a.Count;
        }

        public async Task<int> TotalSubject()
        {
            var a = await _context.Subjects.ToListAsync();
            return a.Count;
        }

        public async Task<int> TotalTeacher()
        {
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");

            return teachers.Count;
        }
    }
}
