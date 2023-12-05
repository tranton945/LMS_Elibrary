using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace LMS_Elibrary.Services
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ElibraryDbContext _context;

        public SubjectRepository(ElibraryDbContext context) 
        {
            _context = context;
        }
        public async Task<Subject> Add(Subject subject)
        {
            var _subject = new Subject
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                Teacher = subject.Teacher,
                Date = DateTime.Now,
                Descriptions = subject.Descriptions,
                ApprovalDocs = 0,
            };
            _context.AddAsync(_subject);
            await _context.SaveChangesAsync();
            return _subject;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Subjects.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            _context.Remove(result);
            return true;
        }

        public async Task<List<Subject>> GetAll()
        {
            var result = await _context.Subjects.OrderByDescending(d => d.Date).ToListAsync();
            return result;
        }

        public async Task<List<string>> GetAllSubjectName()
        {
            var result = await _context.Subjects
                                .Select(s => s.SubjectName)
                                .OrderBy(str => str)
                                .Distinct()
                                .ToListAsync();
            return result;
        }

        public async Task<List<string>> GetAllTeacher(List<AccountWithRolesDto> listAccount)
        {
            //var account = await _account.GetAllAccountRole();
            //var teacherNames = listAccount
            //        .Where(dto => dto.Roles.Any(role => role == "Teacher"))
            //        .Select(dto => dto.Name)
            //        .OrderBy(str => str)
            //        .ToList();
            var result = await _context.Subjects
                    .Select(s => s.Teacher)
                    .OrderBy(str => str)
                    .Distinct()
                    .ToListAsync();
            if (result.Count() == 0 || result == null)
            {
                return new List<string>();
            }
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
        public async Task<List<Subject>> GetSubjectBySchoolYear(string schoolYear)
        {
            var _schoolYear = schoolYear.Split("-");
            // get frist year and convert to int
            var year = int.Parse(_schoolYear[0]);
            var result = await _context.Subjects
                            .Where(s => s.Date.Year == year)
                            .OrderByDescending(d => d.Date)
                            .ToListAsync();
            return result;
        }

        public async Task<List<Subject>> GetSubjectByApprove(int id)
        {
            //// chờ phê duyệt
            //if(id == 0)
            //{
            //    var result0 = await _context.Subjects
            //                    .Where(s => s.Approved == null)
            //                    .OrderByDescending(d => d.Date)
            //                    .ToListAsync();
            //}
            //// Đã phê duyệt
            //if (id == 1)
            //{
            //    var result1 = await _context.Subjects
            //                    .Where(s => s.Approved != null)
            //                    .OrderByDescending(d => d.Date)
            //                    .ToListAsync();
            //}
            //// Phê duyệt
            //if (id == 2)
            //{
            //    var result2 = await _context.Subjects
            //                    .Where(s => s.Approved == true)
            //                    .OrderByDescending(d => d.Date)
            //                    .ToListAsync();
            //}
            //// Hủy
            //if (id == 3)
            //{
            //    var result3 = await _context.Subjects
            //                    .Where(s => s.Approved == false)
            //                    .OrderByDescending(d => d.Date)
            //                    .ToListAsync();
            //}

            return new List<Subject> { };

        }

        public async Task<List<Subject>> GetSubjectByName(string _subjectName)
        {
            var result = await _context.Subjects
                                .Where(s => s.SubjectName == _subjectName)
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
            if(result == null)
            {
                return new List<Subject> { };
            }
            return result;
        }

        public async Task<List<Subject>> GetSubjectByTeacher(string teacher)
        {
            var result = await _context.Subjects
                    .Where(s => s.Teacher == teacher)
                    .OrderByDescending(d => d.Date)
                    .ToListAsync();
            if (result == null)
            {
                return new List<Subject>();
            }
            return result;

        }

        public async Task<List<Subject>> Search(string searchString)
        {
            var result = await _context.Subjects
                    .Where(s => s.Teacher.Contains(searchString) || s.SubjectId.Contains(searchString) || s.SubjectName.Contains(searchString))
                    .OrderByDescending(d => d.Date)
                    .ToListAsync();
            if (result == null)
            {
                return new List<Subject>();
            }
            return result;
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
    }
}
