using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace LMS_Elibrary.Services
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _user;
        private readonly ISubjectRepository _subjectRepository;

        public DocumentRepository(ElibraryDbContext context, GetUser user, ISubjectRepository subjectRepository) 
        {
            _context = context;
            _user = user;
            _subjectRepository = subjectRepository;
        } 
        public async Task<Document> Add(Document document, IFormFile file)
        {
            var isuser = await _user.user();
            var doc = new Document
            {
                Type = document.Type,
                Creator = isuser.Name,
                Date= DateTime.Now,
                Approved = null,
                Approver = null,
                ApproveDate = null,
                Note = null,
                LectureID = document.LectureID,
                Updater = isuser.Name,
                LastUpdate = DateTime.Now
            };
            _context.Documents.Add(doc);
            await _context.SaveChangesAsync();

            var _file = new Data.File
            {
                FileName = file.FileName,
                FileData = await ConvertFormFileToByteArray(file),
                FileType = Path.GetExtension(file.FileName),
                FileSize = (int)file.Length,
                DocumentId = doc.Id,
                PrivateFilesId = null
            };
            _context.Files.Add(_file);
            await _context.SaveChangesAsync();

            // get subjectId to call UpdateApproveDoc()
            var Document = await _context.Documents
                                    .Include(a => a.Lecture)
                                    .ThenInclude(a => a.Topic)
                                    .ThenInclude(a => a.Subject)
                                    .SingleOrDefaultAsync(d => d.Id == doc.Id);

            var subId = Document.Lecture.Topic.Subject.Id;
            await _subjectRepository.UpdateApproveDoc(subId);
            return doc;
        }
        private async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<bool> ApproveDoc(int id)
        {
            var result = await _context.Documents
                                    .Include(a => a.Lecture)
                                    .ThenInclude(a => a.Topic)
                                    .ThenInclude(a => a.Subject)
                                    .SingleOrDefaultAsync(x => x.Id == id);
            if (result == null || result.Approved != null)
            {
                return false;
            }
            var isuser = await _user.user();
            result.Approved = true;
            result.Approver = isuser.Name;
            result.ApproveDate = DateTime.Now.Date;

            var subId = result.Lecture.Topic.Subject.Id;
            await _subjectRepository.UpdateApproveDoc(subId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Documents.Include(a => a.File).SingleOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                return false;
            }
            _context.RemoveRange(result.File);
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DoNotApproveDoc(DoNotApproveDocument doNotApproveDocument)
        {
            var result = await _context.Documents
                                    .Include(a => a.Lecture)
                                    .ThenInclude(a => a.Topic)
                                    .ThenInclude(a => a.Subject)
                                    .SingleOrDefaultAsync(x => x.Id == doNotApproveDocument.DocumentId);
            if (result == null || result.Approved != null)
            {
                return false;
            }
            result.Approved = false;
            result.Approver = doNotApproveDocument.Approver;
            result.ApproveDate = doNotApproveDocument.ApproveDate;

            var subId = result.Lecture.Topic.Subject.Id;
            await _subjectRepository.UpdateApproveDoc(subId);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<DoNotApproveDocument> PopUpDoNotApproveDoc(int docId)
        {
            var isuser = await _user.user();
            var result = new DoNotApproveDocument
            {
                DocumentId = docId,
                ApproveDate = DateTime.Now.Date,
                Approver = isuser.Name,
                Note = null,
                SendNotificaion = true
            };
            return result;
        }

        public async Task<List<Document>> GetAll()
        {
            var result = await _context.Documents
                                    .Include(s => s.File)
                                    .Where(a => a.File.PrivateFile == null)
                                    .ToListAsync();
            return result;
        }

        public async Task<List<DocumentInfo>> GetAllDocInfor()
        {
            var result = await _context.Documents
                                    .Include(a => a.Lecture)
                                        .ThenInclude(a => a.Topic)
                                            .ThenInclude(a => a.Subject)
                                    .Where(doc => doc.File != null && doc.File.FileName != null)
                                    .Select(doc => new DocumentInfo
                                    {
                                        Doc = doc,
                                        SubjectName = doc.Lecture.Topic.Subject.SubjectName,
                                        DocName = doc.File.FileName
                                    })
                                    .OrderByDescending(id => id.Doc.Date)
                                    .ToListAsync();
            return result;
        }

        public async Task<Document> GetById(int id)
        {
            var result = await _context.Documents.Include(a => a.File).SingleOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return new Document();
            }
            return result;
        }

        public async Task<DocumentInfo> GetDocInforById(int id)
        {
            var result = await _context.Documents
                                    .Include(a => a.Lecture)
                                        .ThenInclude(a => a.Topic)
                                            .ThenInclude(a => a.Subject)
                                    .Select(doc => new DocumentInfo
                                    {
                                        Doc = doc,
                                        SubjectName = doc.Lecture.Topic.Subject.SubjectName,
                                        DocName = doc.File.FileName
                                    })
                                    .SingleOrDefaultAsync(i => i.Doc.Id == id);
            if (result == null)
            {
                return new DocumentInfo();
            }
            return result;
        }

        public async Task<bool> Update(Document document, int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return false;
            }
            if(document != null)
            {
                result.Type = document.Type ?? result.Type;
                result.LectureID = document.LectureID ?? result.LectureID;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Document>> Search(string searchString)
        {
            var result = await _context.Documents
                            .Include(a => a.Lecture)
                            .ThenInclude(a => a.Topic)
                            .ThenInclude(a => a.Subject)
                            .Where(s => s.Lecture.Topic.Subject.Teacher.Contains(searchString) ||
                                        s.Lecture.Topic.Subject.SubjectId.Contains(searchString) ||
                                        s.Lecture.Topic.Subject.SubjectName.Contains(searchString))
                            .OrderByDescending(d => d.Date)
                            .ToListAsync();
            if (result == null)
            {
                return new List<Document>();
            }
            return result;
        }

        public async Task<List<Document>> GetDocByName(string _docName)
        {
            var result = await _context.Documents
                            .Include(a => a.Lecture)
                            .ThenInclude(a => a.Topic)
                            .ThenInclude(a => a.Subject)
                            .Where(s => s.Lecture.Topic.Subject.SubjectName == _docName)
                            .OrderByDescending(d => d.Date)
                            .ToListAsync();
            if (result == null)
            {
                return new List<Document>();
            }
            return result;
        }

        public async Task<List<string>> GetAllDocName()
        {
            var result = await _context.Documents
                            .Include(a => a.Lecture)
                            .ThenInclude(a => a.Topic)
                            .ThenInclude(a => a.Subject)
                            .Select(s => s.Lecture.Topic.Subject.SubjectName)
                            .OrderBy(str => str)
                            .Distinct()
                            .ToListAsync();

            if (result == null)
            {
                return new List<string>();
            }
            return result;
        }

        public async Task<List<Document>> GetDocByTeacher(string teacher)
        {
            var result = await _context.Documents
                            .Include(a => a.Lecture)
                            .ThenInclude(a => a.Topic)
                            .ThenInclude(a => a.Subject)
                            .Where(s => s.Lecture.Topic.Subject.Teacher == teacher)
                            .OrderByDescending(d => d.Date)
                            .ToListAsync();
            if (result == null)
            {
                return new List<Document>();
            }
            return result;
        }

        public async Task<List<string>> GetAllTeacher()
        {
            var result = await _context.Documents
                            .Include(a => a.Lecture)
                            .ThenInclude(a => a.Topic)
                            .ThenInclude(a => a.Subject)
                            .Select(s => s.Lecture.Topic.Subject.Teacher)
                            .OrderBy(str => str)
                            .Distinct()
                            .ToListAsync();
            if (result.Count() == 0 || result == null)
            {
                return new List<string>();
            }
            return result;
        }

        public Task<List<string>> GetApproveType()
        {
            List<string> result = new List<string>
            {
                "Tất cả tình trạng",
                "Chờ phê duyệt",
                "Đã phê duyệt",
                "Phê duyệt",
                "Đã hủy"
            };
            return Task.FromResult(result);
        }
        public async Task<List<Document>> GetDocByApprove(string type)
        {
            // chờ phê duyệt
            if (type.ToLower() == "chờ phê duyệt")
            {
                var result0 = await _context.Documents
                                .Where(s => s.Approved == null)
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
                return result0;
            }
            // Đã phê duyệt
            if (type.ToLower() == "đã phê duyệt")
            {
                var result1 = await _context.Documents
                                .Where(s => s.Approved != null)
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
                return result1;
            }
            // Phê duyệt
            if (type.ToLower() == "phê duyệt")
            {
                var result2 = await _context.Documents
                                .Where(s => s.Approved == true)
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
                return result2;
            }
            // Hủy
            if (type.ToLower() == "đã hủy")
            {
                var result3 = await _context.Documents
                                .Where(s => s.Approved == false)
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
                return result3;
            }
            if (type.ToLower() == "tất cả tình trạng")
            {
                var result4 = await _context.Documents
                                .OrderByDescending(d => d.Date)
                                .ToListAsync();
                return result4;
            }

            return new List<Document>();
        }

        public async Task<List<string>> GetSchoolYear()
        {
            var uploadYears = _context.Documents.Select(doc => doc.Date.Year).Distinct().OrderBy(year => year).ToList();
            if (uploadYears.Count() == 0)
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

        public async Task<List<Document>> GetDocBySchoolYear(string schoolYear)
        {
            var _schoolYear = schoolYear.Split("-");
            // get frist year and convert to int
            var year = int.Parse(_schoolYear[0]);
            var result = await _context.Documents
                            .Where(s => s.Date.Year == year)
                            .OrderByDescending(d => d.Date)
                            .ToListAsync();
            return result;
        }

        public async Task<List<Data.File>> DownloadDocFile(List<int> ints)
        {
            var result = await _context.Files
                                .Include(a => a.Document)
                                .Where(f => ints.Contains(f.DocumentId ?? 0) && f.FileData != null && f.FileData.Length > 0)
                                .OrderByDescending(f => f.Document.Date)
                                .ToListAsync();

            return result;
        }
    }
}
