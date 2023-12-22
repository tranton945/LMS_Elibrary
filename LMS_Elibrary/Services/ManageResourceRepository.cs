using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class ManageResourceRepository : IManageResourceRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _user;

        public ManageResourceRepository(ElibraryDbContext context, GetUser user) 
        {
            _context = context;
            _user = user;
        }

        //manage Lecture And Resource
        private List<ManageDocRoleTeacherDTO> CreateListManageDocRoleTeacherDTO(List<Document> documents)
        {
            var DocumentTeacher = documents.Select(sub => new ManageDocRoleTeacherDTO
            {
                DocumentId = sub.Id,
                FileName = sub.File.FileName,
                FileType = sub.File.FileType,
                SubjectName = sub.Lecture?.Topic?.Subject?.SubjectName ?? null,
                FileSize = sub.File.FileSize
            }).ToList();
            return DocumentTeacher;
        }
        public async Task<List<ManageDocRoleTeacherDTO>> ManageGetAllResource()
        {
            var result = await _context.Documents
                .Include(a => a.File)
                .Include(a => a.Lecture)
                .ThenInclude(a => a.Topic)
                .ThenInclude(a => a.Subject)
                .Where(a => a.Type == "Tài nguyên" || a.SubjectId != null)
                .OrderByDescending(a => a.LastUpdate)
                .ToListAsync();
            var DTO = CreateListManageDocRoleTeacherDTO(result);

            return DTO;
        }

        public async Task<List<string>> ManageGetSubjectName()
        {
            var result = await _context.Subjects.Select(a => a.SubjectName).ToListAsync();
            result.Insert(0, "Tất cả môn học");
            return result;
        }

        public async Task<List<ManageDocRoleTeacherDTO>> ManageGetBySubjectName(string SubjectName)
        {
            if (SubjectName == "Tất cả môn học")
            {
                return await ManageGetAllResource();
            };
            var result = await _context.Documents
                        .Include(a => a.File)
                        .Include(a => a.Lecture)
                        .ThenInclude(a => a.Topic)
                        .ThenInclude(a => a.Subject)
                        .Where(a => a.Type == "Tài nguyên" || a.SubjectId != null)
                        .Where(a => a.Lecture.Topic.Subject.SubjectName == SubjectName)
                        .ToListAsync();
            var DTO = CreateListManageDocRoleTeacherDTO(result);

            return DTO;
        }

        public async Task<List<ManageDocRoleTeacherDTO>> ManageSearch(string searchString)
        {
            var doc = await _context.Documents
                    .Include(a => a.File)
                    .Include(a => a.Lecture)
                    .ThenInclude(a => a.Topic)
                    .ThenInclude(a => a.Subject)
                    .ToListAsync();
            var DTO = CreateListManageDocRoleTeacherDTO(doc);
            var result = DTO.Where(a => (a.FileName != null && a.FileName.Contains(searchString)) ||
                                        (a.SubjectName?.Contains(searchString) ?? false) ||
                                        (a.Updater?.Contains(searchString) ?? false)).ToList();
            return result;
        }
        private async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public async Task<bool> ManageUpload(List<IFormFile> files, string subjectName)
        {
            if (subjectName == "Tùy chọn môn học")
            {
                return false;
            }
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.SubjectName == subjectName);
            if (subject == null)
            {
                return false;
            }
            var isuser = await _user.user();

            foreach (var file in files)
            {
                var doc = new Document
                {
                    Type = "Tài nguyên",
                    Creator = isuser.Name,
                    Date = DateTime.Now,
                    Approved = null,
                    Approver = null,
                    ApproveDate = null,
                    Note = null,
                    LectureID = null,
                    SubjectId = subject.Id,
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
            }
            return true;
        }

        public async Task<List<Data.File>> ManageDownload(List<int> DocIds)
        {
            var result = await _context.Files
                    .Include(a => a.Document)
                    .Where(f => DocIds.Contains(f.DocumentId ?? 0) && f.FileData != null && f.FileData.Length > 0)
                    .OrderByDescending(f => f.Document.Date)
                    .ToListAsync();

            return result;
        }

        public async Task<Data.File> ManagePreview(int fileId)
        {
            var result = await _context.Files.SingleOrDefaultAsync(a => a.Id == fileId);
            if (result == null)
            {
                return new Data.File();
            };
            return result;
        }

        public async Task<bool> ManageChangeName(int fileId, string newName)
        {
            var result = await _context.Files.Include(d => d.Document).SingleOrDefaultAsync(a => a.Id == fileId);
            if (result == null)
            {
                return false;
            };
            var isusser = await _user.user();
            result.FileName = newName;
            result.Document.Updater = isusser.Name;
            result.Document.LastUpdate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ManageAddToSubject(string lectureName, List<string>? classRoomNames)
        {
            if (!classRoomNames.Any())
            {
                return false;
            }
            var _classRooms = await _context.ClassRooms
                .Where(classRoom => classRoomNames.Contains(classRoom.ClassRoomName))
                .ToListAsync();
            var _lecture = await _context.Lectures.SingleOrDefaultAsync(a => a.Title == lectureName);
            if (_lecture == null)
            {
                return false;
            }
            foreach (var _classRoom in _classRooms)
            {
                var link = new ClassRoomLectures
                {
                    LectureID = _lecture.Id,
                    ClassRoomID = _classRoom.Id
                };
                _context.ClassRoomLectures.Add(link);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ManageDeleteFile(List<int> DocIds)
        {
            var result = await _context.Documents
                            .Include(f => f.File)
                            .Where(d => DocIds.Contains(d.Id))
                            .ToListAsync();
            if (!result.Any())
            {
                return false;
            }
            _context.Documents.RemoveRange(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> ManageGetSubjectNamePopup()
        {
            var isuser = await _user.user();
            var result = await _context.Subjects
                        .Where(subject => subject.Teachers.Any(teacher => teacher.UserId == isuser.Id))
                        .Select(subject => subject.SubjectName)
                        .ToListAsync();
            result.Insert(0, "Tùy chọn môn học");
            return result;
        }

        public async Task<List<string>> ManageGetTopicBySubjectName(string subjectName)
        {
            var result = await _context.Subjects.Include(a => a.Topics).SingleOrDefaultAsync(a => a.SubjectName == subjectName);
            if (result == null)
            {
                return new List<string>
                {
                    "Subject Not Found"
                };
            }
            var topics = result.Topics.Select(a => a.TopicName).ToList();
            return topics;
        }

        public async Task<List<string>> ManageGetLectureByTopicName(string topicName)
        {
            var topic = await _context.Topics.SingleOrDefaultAsync(a => a.TopicName == topicName);
            var result = await _context.Lectures.Where(a => a.TopicId == topic.Id).ToListAsync();
            var _lecture = result.Select(a => a.Title).ToList();
            return _lecture;
        }
    }
}
