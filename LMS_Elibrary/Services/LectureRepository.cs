using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class LectureRepository : ILectureRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly ISubjectRepository _subjectRepository;

        public LectureRepository(ElibraryDbContext context, GetUser getUser, ISubjectRepository subjectRepository) 
        {
            _context = context;
            _getUser = getUser;
            _subjectRepository = subjectRepository;
        }
        public async Task<Lecture> Add(CreateLectureModel lecture)
        {
            var isTitleDuplicate = await _context.Lectures.AnyAsync(a => a.Title == lecture.Title);
            if(isTitleDuplicate)
            {
                return null;
            }
            var _lecture = new Lecture
            {
                Title= lecture.Title,
                Descriptions=lecture.Descriptions,
                TopicId=lecture.TopicId,
                BlockStudents = lecture.BlockStudent
            };
            _context.Lectures.Add(_lecture);
            await _context.SaveChangesAsync();
            return _lecture;
        }

        public async Task<Lecture> AddLectureAndDocument(LectureAndDocumentInput lectureAndDocumentInput)
        {
            var isTitleDuplicate = await _context.Lectures.AnyAsync(a => a.Title == lectureAndDocumentInput.Lecture.Title);
            if (isTitleDuplicate)
            {
                return null;
            }
            var _lecture = new Lecture
            {
                Title = lectureAndDocumentInput.Lecture.Title,
                Descriptions = lectureAndDocumentInput.Lecture.Descriptions,
                TopicId = lectureAndDocumentInput.Lecture.TopicId,
                BlockStudents = lectureAndDocumentInput.Lecture.BlockStudent
            };
            _context.Add(lectureAndDocumentInput.Lecture);
            await _context.SaveChangesAsync();

            var issuer = await _getUser.user();

            foreach (var lesson in lectureAndDocumentInput.Lessons)
            {

                var doc = new Document
                {
                    Type = "Bài giảng",
                    Creator = issuer.Name,
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
                    FileName = lesson.FileName,
                    FileData = await ConvertFormFileToByteArray(lesson),
                    FileType = Path.GetExtension(lesson.FileName),
                    FileSize = (int)lesson.Length,
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
            }

            foreach (var resource in lectureAndDocumentInput.Resources)
            {

                var doc = new Document
                {
                    Type = "Tài nguyên",
                    Creator = issuer.Name,
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
                    FileName = resource.FileName,
                    FileData = await ConvertFormFileToByteArray(resource),
                    FileType = Path.GetExtension(resource.FileName),
                    FileSize = (int)resource.Length,
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
            }


            var documents = await _context.Documents.Where(a => lectureAndDocumentInput.DocIds.Contains(a.Id)).ToListAsync();
            foreach( var document in documents)
            {
                document.LectureID = _lecture.Id;
            }
            await _context.SaveChangesAsync();
            if(lectureAndDocumentInput.AssignDocument == true || lectureAndDocumentInput.ClassRooms.Count() != 0)
            {
                await AssignDocument(_lecture.Id, lectureAndDocumentInput.ClassRooms);
            }
            return _lecture;
        }


        private async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<bool> BlockStudent(int id, bool blockStudent)
        {
            var result = await _context.Lectures.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            result.BlockStudents = blockStudent;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Lectures.SingleOrDefaultAsync(i => i.Id == id);
            if(result == null) 
            {
                return false;
            }
            var documents = await _context.Documents
                         .Include(a => a.Lecture)
                         .Where(s => s.Lecture.Id == id)
                         .OrderByDescending(id => id.Date)
                         .ToListAsync();
            // update all LectureID of document to null
            foreach (var document in documents)
            {
                document.LectureID = null;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Lecture>> GetAll()
        {
            var result = await _context.Lectures.ToListAsync();
            return result;
        }

        public async Task<Lecture> GetById(int id)
        {
            var result = await _context.Lectures
                .Include(a => a.Documents)
                .ThenInclude(a => a.File)
                .SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return new Lecture();;
            }
            return result;
        }

        public async Task<bool> Update(CreateLectureModel lecture, int id)
        {
            var result = await _context.Lectures.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            if(lecture != null) 
            {
                result.Title = lecture.Title ?? result.Title;
                result.Descriptions = lecture.Descriptions ?? result.Descriptions;
                result.TopicId = (lecture.TopicId != null || lecture.TopicId != 0) ? lecture.TopicId : result.TopicId;
                result.BlockStudents = lecture.BlockStudent ?? result.BlockStudents;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> AssignDocument(int LectureId, List<string> classRooms)
        {
            if (classRooms != null && !classRooms.Any())
            {
                return false;
            }
            var _classRooms = await _context.ClassRooms
                .Where(classRoom => classRooms.Contains(classRoom.ClassRoomName))
                .ToListAsync();
            foreach (var _classRoom in _classRooms)
            {
                var link = new ClassRoomLectures
                {
                    LectureID = LectureId,
                    ClassRoomID = _classRoom.Id
                };
                _context.ClassRoomLectures.Add(link);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
