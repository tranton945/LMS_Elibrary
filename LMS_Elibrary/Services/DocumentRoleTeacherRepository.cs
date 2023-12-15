using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;

namespace LMS_Elibrary.Services
{
    public class DocumentRoleTeacherRepository : IDocumentRoleTeacherRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _user;
        private readonly ISubjectRepository _subjectRepository;

        public DocumentRoleTeacherRepository(ElibraryDbContext context, GetUser getUser, ISubjectRepository subjectRepository) 
        {
            _context = context;
            _user = getUser;
            _subjectRepository = subjectRepository;
        }
        public async Task<PopupAddDocmentDTO> PopupAddLecture(int subId)
        {
            var result = await _context.Subjects
                                    .Include(a => a.Topics)
                                    .SingleOrDefaultAsync(x => x.Id == subId);
            if(result == null)
            {
                return new PopupAddDocmentDTO();
            }
            var _topices = result.Topics.Select(f => f.TopicName).ToList();
            var popup = new PopupAddDocmentDTO
            {
                SubjectId = subId,
                SubjectName = result.SubjectName,
                Topics = _topices
            };
            return popup;
        }
        public async Task<List<Document>> AddLecture(AddDocRoleTeacherModel addDocRoleTeacherModel, List<IFormFile> files)
        {
            var isuser = await _user.user();
            var toppicId = await _context.Topics.SingleOrDefaultAsync(c => c.TopicName == addDocRoleTeacherModel.Topic);
            var _lecture = new Lecture
            {
                Title = addDocRoleTeacherModel.Lecture,
                Descriptions = null,
                TopicId = toppicId.Id,
            };
            _context.Lectures.Add(_lecture);
            await _context.SaveChangesAsync();

            List<Document> result = new List<Document>();
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
                    LectureID = _lecture.Id,
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

                result.Add(doc);
            }

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
        public async Task<PopupAddDocmentDTO> PopupAddResources(int subId)
        {
            var result = await _context.Subjects
                        .Include(a => a.Topics)
                        .ThenInclude(a => a.Lecture)
                        .SingleOrDefaultAsync(x => x.Id == subId);
            if (result == null)
            {
                return new PopupAddDocmentDTO();
            }
            var _topices = result.Topics.Select(f => f.TopicName).ToList();
            var _lecture = result.Topics
                                .SelectMany(f => f.Lecture.Select(lecture => lecture.Title))
                                .ToList();
            var popup = new PopupAddDocmentDTO
            {
                SubjectId = subId,
                SubjectName = result.SubjectName,
                Topics = _topices,
                Lectures = _lecture
            };
            return popup;
        }
        public async Task<List<Document>> AddResources(AddDocRoleTeacherModel addDocRoleTeacherModel, List<IFormFile> files)
        {
            var isuser = await _user.user();
            var lectureId = await _context.Lectures.SingleOrDefaultAsync(c => c.Title == addDocRoleTeacherModel.Lecture);
            if(lectureId == null)
            {
                return null;
            }
            List<Document> result = new List<Document>();
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
                    LectureID = lectureId.Id,
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

                result.Add(doc);
            }

            return result;
        }

        public async Task<bool> Delete(List<int> id)
        {
            var result = await _context.Documents
                             .Include(s => s.File)
                             .Where(f => id.Contains(f.Id))
                             .ToListAsync();
            // Kiểm tra nếu có Document cần xóa
            if (result.Any())
            {
                // Xóa các Document, Entity Framework sẽ tự động xóa File liên quan
                _context.Documents.RemoveRange(result);

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                return true;
            }
            return false;   
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

        public async Task<List<DocumentRoleTeacherDTO>> GetAllBySubjectId(int subId)
        {
            var result = await _context.Documents
                                        .Include(s => s.File)
                                        .Include(a => a.Lecture)
                                            .ThenInclude(a => a.Topic)
                                                .ThenInclude(a => a.Subject)
                                         .Where(a => a.Lecture.Topic.Subject.Id == subId)
                                         .ToListAsync();
            var issuer = await _user.user();
            var DocumentTeacher = CreateListDocumentTeacher(result);
            var newSubAccessHistory = new SubAccessHistory
            {
                UserId =  issuer.Id,
                AccessDate = DateTime.Now,
                SubjectId = subId,
            };
            _context.SubAccessHistories.Add(newSubAccessHistory);
            await _context.SaveChangesAsync();
            return DocumentTeacher;
        }
        private List<DocumentRoleTeacherDTO> CreateListDocumentTeacher(List<Document> documents)
        {
            var DocumentTeacher = documents.Select(sub => new DocumentRoleTeacherDTO
            {
                DocumentId = sub.Id,
                FileName = sub.File.FileName,
                Type = sub.Type,
                Date = sub.Date,
                Approver = sub.Approver,
                Status = (sub.Approved == null) ? "Chưa phê duyệt" : (sub.Approved == true) ? "Đã phê duyệt" : "Đã hủy",
                Note = sub.Note
            }).ToList();
            return DocumentTeacher;
        }

        public Task<List<string>> GetStatus()
        {
            var status = new List<string>
            {
                "Tất cả tình trạng",
                "Đã phê duyệt",
                "Chờ phê duyệt",
                "Đã hủy"
            };

            return Task.FromResult(status);
        }

        public async Task<List<DocumentRoleTeacherDTO>> GetDocByStatus(string type)
        {
            var result = await _context.Documents
                            .Include(s => s.File)
                            .Include(a => a.Lecture)
                                .ThenInclude(a => a.Topic)
                                    .ThenInclude(a => a.Subject)
                             .ToListAsync();
            var DocumentTeacher = CreateListDocumentTeacher(result);
            if(type.ToLower() == "tất cả tình trạng")
            {
                return DocumentTeacher;
            }

            return DocumentTeacher.Where(s => s.Status.ToLower() == type.ToLower()).ToList(); 
        }

        public async Task<Data.File> RreviewPopup(int docId)
        {
            var result = await _context.Documents
                    .Include(s => s.File)
                    .SingleOrDefaultAsync(f => f.Id == docId);
            if(result == null) 
            {
                return new Data.File();
            }
            return result.File;

        }

        public async Task<List<DocumentRoleTeacherDTO>> Search(string searchString)
        {
            var result = await _context.Documents
                                .Include(s => s.File)
                                .Where(f => f.File.FileName.Contains(searchString))
                                .ToListAsync();
            var DocumentTeacher = CreateListDocumentTeacher(result);
            return DocumentTeacher;
        }

        public async Task<List<DocumentRoleTeacherDTO>> SortDocument(string columnName, bool isAscending)
        {
            var result = await _context.Documents
                                    .Include(s => s.File)
                                    .ToListAsync();
            var DocumentTeacher = CreateListDocumentTeacher(result);

            var sortProperties = new Dictionary<string, Func<DocumentRoleTeacherDTO, object>>
            {
                { "ngày gửi phê duyệt", a => a.Date },
                { "tên tài liệu", a => a.FileName },
                { "phân loại", a => a.Type },
                { "người phê duyệt", a => a.Approver },
                { "tình trạng phê duyệt", a => a.Status },
                { "ghi chú", a => a.Note },
            };

            if (sortProperties.TryGetValue(columnName.ToLower(), out var sortProperty))
            {
                return isAscending
                    ? DocumentTeacher.OrderBy(sortProperty).ToList()
                    : DocumentTeacher.OrderByDescending(sortProperty).ToList();
            }

            return DocumentTeacher;
        }


    }
}
