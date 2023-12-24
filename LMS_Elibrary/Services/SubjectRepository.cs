using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace LMS_Elibrary.Services
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubjectRepository(ElibraryDbContext context, GetUser getUser, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _getUser = getUser;
            _userManager = userManager;
        }
        public async Task<Subject> Add(Subject subject)
        {
            var isDuplicate = await _context.Subjects.AnyAsync(a => a.SubjectName == subject.SubjectName);
            if (isDuplicate)
            {
                return null;
            }
            var _subject = new Subject
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                Teacher = subject.Teacher,
                Date = DateTime.Now,
                Descriptions = subject.Descriptions,
                ApprovalDocs = null
            };
            _context.AddAsync(_subject);
            await _context.SaveChangesAsync();
            return _subject;
        }

        public async Task<bool> Delete(int id)
        {
            var subject = await _context.Subjects
                            .Include(s => s.SubjectOtherInformations)
                            .Include(s => s.SubAccessHistories)
                            .Include(c => c.Classes)
                            .Include(s => s.Topics)
                            .ThenInclude(t => t.Lecture)
                            .ThenInclude(l => l.Documents)
                            .SingleOrDefaultAsync(i => i.Id == id);
            if (subject == null)
            {
                return false;
            }
            var documents = await _context.Documents
                         .Include(a => a.Lecture)
                             .ThenInclude(a => a.Topic)
                                 .ThenInclude(a => a.Subject)
                         .Where(s => s.Lecture.Topic.Subject.Id == id)
                         .OrderByDescending(id => id.Date)
                         .ToListAsync();
            // update all LectureID of document to null
            foreach (var document in documents)
            {
                document.LectureID = null;
            }
            _context.Lectures.RemoveRange(subject.Topics.SelectMany(t => t.Lecture));
            _context.Topics.RemoveRange(subject.Topics);
            _context.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SubjectLeadershipDTO>> GetAll()
        {
            var subjects = await _context.Subjects
                //.Include(a => a.Topics)
                //    .ThenInclude(a => a.Lecture)
                //        .ThenInclude(a => a.Documents)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            var SubjectLeadership = CreateListSubjectLeadership(subjects);

            return SubjectLeadership;
        }
        public async Task UpdateApproveDoc(int Id)
        {
            var subject = await _context.Subjects
                .Include(a => a.Topics)
                    .ThenInclude(a => a.Lecture)
                        .ThenInclude(a => a.Documents)
                .SingleOrDefaultAsync(s => s.Id == Id);

            if (subject != null)
            {
                int totalDocuments = subject.Topics
                    .SelectMany(t => t.Lecture.SelectMany(l => l.Documents))
                    .Count();

                int approvedDocuments = subject.Topics
                    .SelectMany(t => t.Lecture.SelectMany(l => l.Documents))
                    .Count(d => d.Approved == true);

                subject.ApprovalDocs = $"{approvedDocuments}/{totalDocuments}";

                _context.Update(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetAllSubjectName()
        {
            var result = await _context.Subjects
                                .Select(s => s.SubjectName)
                                .OrderBy(str => str)
                                .Distinct()
                                .ToListAsync();
            result.Insert(0, "Tất cả môn học");
            return result;
        }

        public async Task<List<string>> GetAllTeacher()
        {
            var result = await _context.Subjects
                    .Select(s => s.Teacher)
                    .OrderBy(str => str)
                    .Distinct()
                    .ToListAsync();
            result.Insert(0, "Tất cả giảng viên");
            return result;
        }

        public async Task<Subject> GetById(int id)
        {
            var result = await _context.Subjects
                                    .Include(a => a.Topics)
                                    .ThenInclude(a =>a.Lecture)
                                    .ThenInclude(a => a.Documents)
                                    .ThenInclude(a => a.File)
                                    .SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return new Subject { };
            }
            return result;
        }

        public async Task<List<string>> GetSchoolYear()
        {
            var uploadYears = _context.Documents.Select(doc => doc.Date.Year).Distinct().OrderBy(year => year).ToList();
            if(uploadYears.Count() == 0)
            {
                return new List<string>();
            }
            var timeRanges = new List<string>();

            for (int i = 0; i < uploadYears.Count; i++)
            {
                var rangeStart = uploadYears[i];
                var rangeEnd = uploadYears[i] + 1;
                var timeRange = $"{rangeStart}-{rangeEnd}";
                timeRanges.Add(timeRange);
            }

            return timeRanges;
        }
        public async Task<List<SubjectLeadershipDTO>> GetSubjectBySchoolYear(string schoolYear)
        {
            var _schoolYear = schoolYear.Split("-");
            // get frist year and convert to int
            var year = int.Parse(_schoolYear[0]);
            var result = await _context.Subjects
                            .Where(s => s.Date.Year == year)
                            .OrderByDescending(d => d.Date)
                            .ToListAsync();
            var SubjectLeadership = CreateListSubjectLeadership(result);
            return SubjectLeadership;
        }

        public async Task<List<SubjectLeadershipDTO>> GetSubjectByName(string _subjectName)
        {
            var result = await _context.Subjects
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
            var SubjectLeadership = CreateListSubjectLeadership(result);

            if(_subjectName.ToLower() == "tất cả môn học")
            {
                return SubjectLeadership;
            }
            var _SubjectLeadership = SubjectLeadership.Where(s => s.SubjectName == _subjectName).ToList();
            return _SubjectLeadership;
        }

        public async Task<List<SubjectLeadershipDTO>> GetSubjectByTeacher(string teacher)
        {
            var result = await _context.Subjects
                    .OrderByDescending(d => d.Date)
                    .ToListAsync();
            var SubjectLeadership = CreateListSubjectLeadership(result);
            if (teacher.ToLower() == "tất cả giảng viên")
            {
                return SubjectLeadership;
            }
            var _SubjectLeadership = SubjectLeadership.Where(s => s.Teacher == teacher).ToList();
            return _SubjectLeadership;

        }

        public async Task<List<SubjectLeadershipDTO>> Search(string searchString)
        {
            var result = await _context.Subjects
                    .Where(s => s.Teacher.Contains(searchString) || s.SubjectId.Contains(searchString) || s.SubjectName.Contains(searchString))
                    .OrderByDescending(d => d.Date)
                    .ToListAsync();

            var SubjectLeadership = CreateListSubjectLeadership(result);

            return SubjectLeadership;
        }

        public async Task<bool> Update(Subject subject, int id)
        {
            var result = await _context.Subjects.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            result.SubjectId = subject.SubjectId ?? result.SubjectId;
            result.SubjectName = subject.SubjectName ?? result.SubjectName;
            result.Teacher = subject.Teacher ?? result.Teacher;
            result.Descriptions = subject.Descriptions ?? result.Descriptions;

            await _context.SaveChangesAsync();
            return true;
        }

        private List<SubjectLeadershipDTO> CreateListSubjectLeadership(List<Subject> subjects)
        {
            var SubjectLeadership = subjects.Select(sub => new SubjectLeadershipDTO
            {
                ID = sub.Id,
                SubjectID = sub.SubjectId,
                SubjectName = sub.SubjectName,
                Teacher = sub.Teacher,
                Date = sub.Date,
                ApproveDoc = sub.ApprovalDocs,
                Status = (sub.ApprovalDocs?.Split("/").Length == 2 && sub.ApprovalDocs.Split("/")[0] == sub.ApprovalDocs.Split("/")[1]) ? "Đã phê duyệt" : "Chờ phê duyệt"
            }).ToList();
            return SubjectLeadership;
        }

        public Task<List<string>> GetApproveDocType()
        {
            List<string> result = new List<string>
            {
                "Tất cả tình trạng",
                "Chờ phê duyệt",
                "Đã phê duyệt",
            };
            return Task.FromResult(result);
        }
        public async Task<List<SubjectLeadershipDTO>> GetSubjectByApproveDocType(string type)
        {
            var result = await _context.Subjects
                        .OrderByDescending(d => d.Date)
                        .ToListAsync();
            var SubjectLeadership = CreateListSubjectLeadership(result);
            if (type.ToLower() == "tất cả tình trạng")
            {
                return SubjectLeadership;
            }
            if (type.ToLower() == "chờ phê duyệt")
            {
                var result1 = SubjectLeadership.Where(a => a.Status.ToLower() == "chờ phê duyệt").ToList();

                return result1;
            }
            if (type.ToLower() == "đã phê duyệt")
            {
                var result2 = SubjectLeadership.Where(a => a.Status.ToLower() == "đã phê duyệt").ToList();

                return result2;
            }

            return new List<SubjectLeadershipDTO>();
        }

        public async Task<bool> AddTeacherToSubject(int subjectId, string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return false;
            }
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.Id == subjectId);
            if (subject == null)
            {
                return false;
            }
            var addTeacher = new Teacher
            {
                SubjectId = subjectId,
                UserId = user.Id,
            };
            _context.Teachers.Add(addTeacher);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AddStudentToSubject(int subjectId, string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return false;
            }
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.Id == subjectId);
            if (subject == null)
            {
                return false;
            }
            var addStudent = new Student
            {
                SubjectId = subjectId,
                UserId = user.Id,
            };
            _context.Students.Add(addStudent);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
