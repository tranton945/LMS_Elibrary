using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class SubjectRoleTeacherRepository : ISubjectRoleTeacherRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public SubjectRoleTeacherRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }

        private List<SubjectTeacherDTO> CreateListSubjectTeacher(List<Subject>? subjects)
        {
            if (subjects == null || subjects.Count == 0)
            {
                return new List<SubjectTeacherDTO>();
            }
            var SubjectTeacher = subjects.Select(sub => new SubjectTeacherDTO
            {
                Id = sub.Id,
                SubjectID = sub.SubjectId,
                SubjectName = sub.SubjectName,
                Descriptions = sub.Descriptions,
                ApproveDoc = sub.ApprovalDocs,
                Status = (sub.ApprovalDocs?.Split("/").Length == 2 && sub.ApprovalDocs.Split("/")[0] == sub.ApprovalDocs.Split("/")[1]) ? "Đã phê duyệt" : "Chờ phê duyệt",
                classRooms = sub.Classes
            }).ToList();
            return SubjectTeacher;
        }

        public async Task<List<SubjectTeacherDTO>> GetAllRoleTeacher()
        {
            var subjects = await _context.Subjects
                            .Include(a => a.Classes)
                            .OrderBy(str => str.SubjectName)
                            .ToListAsync();

            var SubjectTeacher = CreateListSubjectTeacher(subjects);

            return SubjectTeacher;
        }

        public async Task<List<SubjectTeacherDTO>> SearchTeacher(string searchString)
        {
            var result = await _context.Subjects
                            .Include(a => a.Classes)
                            .Where(s => s.SubjectId.Contains(searchString) || s.SubjectName.Contains(searchString))
                            .OrderBy(str => str.SubjectName)
                            .ToListAsync();

            var SubjectTeacher = CreateListSubjectTeacher(result);

            return SubjectTeacher;
        }

        public Task<List<string>> GetFilterTeacherString()
        {
            List<string> result = new List<string>
            {
                "Tên môn học",
                "Lần truy cập gần nhất",
            };
            return Task.FromResult(result);
        }
        public async Task<List<SubjectTeacherDTO>> GetSubjectByNameRoleTeacher(string type)
        {

            var result = await _context.Subjects
                                    .Include(a => a.Classes)
                                    .OrderBy(str => str.SubjectName)
                                    .ToListAsync();
            var isuser = await _getUser.user();
            var SubjectTeacher = CreateListSubjectTeacher(result);
            if (type.ToLower() == "tên môn học")
            {
                return SubjectTeacher;
            }
            if (type.ToLower() == "lần truy cập gần nhất")
            {
                var accessHistory = await _context.SubAccessHistories
                                .Where(sa => sa.UserId == isuser.Id)
                                .OrderByDescending(sa => sa.AccessDate)
                                .GroupBy(sa => sa.SubjectId)
                                .Select(group => group.First())
                                .ToListAsync();
                // danh sách Subject đã được truy cập
                var SubjectIds = accessHistory.Select(a => a.Id).ToList();
                // danh sách Subject chưa được truy cập
                var subjectIdsNotAccessed = await _context.Subjects
                                .Where(a => !SubjectIds.Contains(a.Id))
                                .Select(a => a.Id)
                                .ToListAsync();
                // gộp hai list ID
                var allSubjectIds = SubjectIds.Concat(subjectIdsNotAccessed).ToList();

                var result1 = await _context.Subjects
                                .Where(a => allSubjectIds.Contains(a.Id))
                                .OrderBy(a => allSubjectIds.IndexOf(a.Id))
                                .ToListAsync();
                var _SubjectTeacher = CreateListSubjectTeacher(result1);
                return _SubjectTeacher;
            }

            return CreateListSubjectTeacher(null);
        }

        public async Task<Subject> SubjectOverview(int subId)
        {
            var isuser = await _getUser.user();
            var result = await _context.Subjects.SingleOrDefaultAsync(a => a.Id == subId);
            if(result == null)
            {
                return new Subject();
            }
            var newSubAccessHistory = new SubAccessHistory
            {
                UserId = isuser.Id,
                SubjectId = subId,
                AccessDate = DateTime.Now,
            };
            _context.SubAccessHistories.Add(newSubAccessHistory);
            await _context.SaveChangesAsync();
            var subject =  new Subject
            {
                Id = result.Id,
                SubjectId = result.SubjectId,
                SubjectName = result.SubjectName,
                Teacher = result.Teacher,
                Descriptions= result.Descriptions
            };
            return subject;
        }

        public async Task<List<Topic>> ListTopic(int subId)
        {
            var result = await _context.Topics
                                    .Include(a => a.Lecture)
                                    .ThenInclude(a => a.Documents)
                                    .ThenInclude(a => a.File)
                                    .Where(i => i.SubjectId == subId)
                                    .ToListAsync();
            return result;
        }

        public Task<List<SubjectTeacherDTO>> QuestAndAnswer(string type)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubjectTeacherDTO>> SubjectNotifiaction(string type)
        {
            throw new NotImplementedException();
        }
    }
}
