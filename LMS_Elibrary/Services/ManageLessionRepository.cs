using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class ManageLessionRepository : IManageLessionRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _user;
        public ManageLessionRepository(ElibraryDbContext context, GetUser user) 
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
        public async Task<List<ManageDocRoleTeacherDTO>> ManageGetAllLession()
        {
            var result = await _context.Documents
                .Include(a => a.File)
                .Include(a => a.Lecture)
                .ThenInclude(a => a.Topic)
                .ThenInclude(a => a.Subject)
                .Where(a => a.Type == "Bài giảng" || a.SubjectId != null)
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
                return await ManageGetAllLession();
            };
            var result = await _context.Documents
                        .Include(a => a.File)
                        .Include(a => a.Lecture)
                        .ThenInclude(a => a.Topic)
                        .ThenInclude(a => a.Subject)
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
                    .Where(a => a.Type == "Bài giảng" || a.SubjectId != null)
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
                    Type = "Bài giảng",
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

        public async Task<Data.File> ManagePreview(int docId)
        {
            var result = await _context.Files.SingleOrDefaultAsync(a => a.Id == docId);
            if (result == null)
            {
                return new Data.File();
            };
            return result;
        }

        public async Task<bool> ManageChangeName(int docId, string newName)
        {
            var result = await _context.Files.Include(d => d.Document).SingleOrDefaultAsync(a => a.Id == docId);
            if (result == null)
            {
                return false;
            };
            var isusser = await _user.user();
            var _newName = newName + result.FileType;
            result.FileName = _newName;
            result.Document.Updater = isusser.Name;
            result.Document.LastUpdate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ManageAddToSubject(int docId, string topic, string lectureName, List<string>? classRoomNames)
        {

            var _topic = await _context.Topics.SingleOrDefaultAsync(a => a.TopicName == topic);
            if (_topic == null)
            {
                return false;
            }
            var _lecture = await _context.Lectures.SingleOrDefaultAsync(a => a.Title == lectureName);
            // lọc trùng lecture title
            if (_lecture != null)
            {
                return false;
            }
            var _doc = await _context.Documents.SingleOrDefaultAsync(a => a.Id == docId);
            if (_doc == null)
            {
                return false;
            }
            var newLecture = new Lecture
            {
                TopicId = _topic.Id,
                Title = lectureName,
                Descriptions = null,
                BlockStudents = false
            };
            _context.Lectures.Add(newLecture);
            await _context.SaveChangesAsync();

            _doc.LectureID = newLecture.Id;
            await _context.SaveChangesAsync();
            // mảng k có phần tử
            if (classRoomNames != null && !classRoomNames.Any())
            {
                return true;
            }
            var _classRooms = await _context.ClassRooms
                .Where(classRoom => classRoomNames.Contains(classRoom.ClassRoomName))
                .ToListAsync();
            foreach (var _classRoom in _classRooms)
            {
                var link = new ClassRoomLectures
                {
                    LectureID = newLecture.Id,
                    ClassRoomID = _classRoom.Id
                };
                _context.ClassRoomLectures.Add(link);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ManageDeleteDoc(List<int> DocIds)
        {
            var result = await _context.Documents
                            .Include(f => f.File)
                            .Where(d => DocIds.Contains(d.Id))
                            .ToListAsync();
            if (result != null&& !result.Any())
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
    }
}
