using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LMS_Elibrary.Services
{
    public class HomeStudentRepository : IHomeStudentRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeStudentRepository(ElibraryDbContext context, GetUser getUser, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _getUser = getUser;
            _userManager = userManager;
        }

        public Task<List<SubjectHomeTeacher>> DownloadResource(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HomeStudentDTO>> GetAll()
        {
            var isuser = await _getUser.user();
            var aq = await _context.Students.Where(a => a.UserId == isuser.Id).ToListAsync();
            var result = new List<HomeStudentDTO>();
            foreach(var item in aq)
            {                
                var sub = await _context.Subjects.Where(a => a.Id == item.Id).ToListAsync();
                foreach(var _item in sub)
                {
                    var _favoriteSubjects = await _context.FavoriteSubjects.Where(a => a.SubjectID == _item.Id && a.UserId == isuser.Id).FirstOrDefaultAsync();
                    var DTO = new HomeStudentDTO
                    {
                        SubjectId = _item.Id,
                        SubjectKey = _item.SubjectId,
                        SubjectName = _item.SubjectName,
                        Teacher = _item.Teacher,
                        IsStart = (_favoriteSubjects != null) ? _favoriteSubjects.IsStar : false
                    };
                    result.Add(DTO);
                }
            }
            return result;
        }

        public async Task<Subject> GetBySubjectId(int subjectID)
        {
            var result = await _context.Subjects
                .Include(a => a.Topics)
                    .ThenInclude(a => a.Lecture)
                        .ThenInclude(a => a.Documents)
                            .ThenInclude(a => a.File)
                .Select(subject => new Subject
                {
                    Id = subject.Id,
                    SubjectId = subject.SubjectId,
                    SubjectName = subject.SubjectName,
                    Teacher = subject.Teacher,
                    Date = subject.Date,
                    Descriptions = subject.Descriptions,
                    ApprovalDocs = subject.ApprovalDocs,
                    SubAccessHistories = subject.SubAccessHistories,
                    Classes = subject.Classes,
                    SubjectOtherInformations = subject.SubjectOtherInformations,
                    Teachers = subject.Teachers,
                    Exams = subject.Exams,
                    FavoriteSubjects = subject.FavoriteSubjects,
                    Students = subject.Students,
                    Topics = subject.Topics.Select(topic => new Topic
                    {
                        Id = topic.Id,
                        TopicName = topic.TopicName,
                        SubjectId = topic.SubjectId,
                        Subject = null,  // Để tránh lặp lại dữ liệu
                        Lecture = topic.Lecture
                            .Where(lecture => lecture.BlockStudents != true)
                            .Select(lecture => new Lecture
                            {
                                Id = lecture.Id,
                                Title = lecture.Title,
                                Descriptions = lecture.Descriptions,
                                BlockStudents = lecture.BlockStudents,
                                TopicId = lecture.TopicId,
                                Topic = null,  // Để tránh lặp lại dữ liệu
                                Documents = lecture.Documents
                                    .Where(document => document.Approved != false && document.Approved != null)
                                    .Select(document => new Document
                                    {
                                        Id = document.Id,
                                        Type = document.Type,
                                        Creator = document.Creator,
                                        Date = document.Date,
                                        Approved = document.Approved,
                                        Approver = document.Approver,
                                        ApproveDate = document.ApproveDate,
                                        Note = document.Note,
                                        Updater = document.Updater,
                                        LastUpdate = document.LastUpdate,
                                        LectureID = document.LectureID,
                                        Lecture = null,  // Để tránh lặp lại dữ liệu
                                        SubjectId = document.SubjectId,
                                        Subject = null,  // Để tránh lặp lại dữ liệu
                                        File = document.File  // Giữ lại File của Documents thỏa mãn điều kiện
                                    }).ToList()
                            }).ToList(),
                        Questions = topic.Questions
                    }).ToList()
                }).SingleOrDefaultAsync(i => i.Id == subjectID);

            if (result == null)
            {
                return new Subject();
            }
            return result;
        }

        public async Task<bool> LikeSubject(int subjectID)
        {
            var isuser = await _getUser.user();
            var like = new FavoriteSubjects
            {
                SubjectID = subjectID,
                UserId = isuser.Id,
                IsStar = true
            };
            _context.FavoriteSubjects.Add(like);
            await _context.AddRangeAsync();
            return true;
        }
        private async Task<List<SubjectNotificationDTO>> CreateClassRoomNotificationDTO(List<SubjectNotification>? notifications)
        {
            if (notifications == null || notifications.Count == 0)
            {
                return new List<SubjectNotificationDTO>();
            }
            List<SubjectNotificationDTO> DTOs = new List<SubjectNotificationDTO>();
            foreach (var notification in notifications)
            {
                var user = await _userManager.FindByIdAsync(notification.CreatorId);
                var _notification = new SubjectNotificationDTO
                {
                    Id = notification.Id,
                    Avatar = user != null ? user.Avatar : string.Empty,
                    UserName = user != null ? user.Name : string.Empty,
                    Date = notification.Date,
                    Content = notification.Content,
                    Title = notification.Title
                };
                DTOs.Add(_notification);
            }
            return DTOs;
        }
        public async Task<List<SubjectNotificationDTO>> Notification(int subjectID)
        {
 
            var resule = await _context.SubjectNotifications.Where(a => a.SubjectId == subjectID).ToListAsync();
            var DTO = await CreateClassRoomNotificationDTO(resule);
            return DTO;
        }
        public async Task<Subject> SubjectOverview(int subId)
        {
            var isuser = await _getUser.user();
            var result = await _context.Subjects.Include(s => s.SubjectOtherInformations).SingleOrDefaultAsync(a => a.Id == subId);
            if (result == null)
            {
                return new Subject();
            }
            var subject = new Subject
            {
                Id = result.Id,
                SubjectId = result.SubjectId,
                SubjectName = result.SubjectName,
                Teacher = result.Teacher,
                Descriptions = result.Descriptions,
                SubjectOtherInformations = result.SubjectOtherInformations,
            };
            return subject;
        }
        public async Task<List<HomeStudentDTO>> Search(string searchString)
        {
            var result = await GetAll();
            var search = result.Where(a => a.SubjectKey.Contains(searchString) || a.SubjectName.Contains(searchString)).ToList();
            return search;
        }

        public async Task<List<HomeStudentDTO>> SortByName()
        {
            var result = await GetAll();
            var a = result.OrderBy(a => a.SubjectName).ToList();
            return a;
        }

        public async Task<List<HomeStudentDTO>> SortByStar(string type)
        {
            var result = await GetAll();
            if(type.ToLower() == "được gắn dấu sao")
            {
                var a = result.Where(a => a.IsStart == true).ToList();
                return a;
            }
            if(type.ToLower() == "không gắn dấu sao")
            {
                var a = result.Where(a => a.IsStart == false).ToList();
                return a;
            }
            return null;
        }
        private async Task<List<QuestionDTO>> CreateListQuestionDTO(List<Questions>? ques)
        {
            if (ques == null || ques.Count == 0)
            {
                return new List<QuestionDTO>();
            }
            List<QuestionDTO> questionList = new List<QuestionDTO>();

            foreach (var q in ques)
            {
                var user = await _userManager.FindByIdAsync(q.UserId);
                var questionDto = new QuestionDTO
                {
                    QuestionId = q.Id,
                    Title = q.Title,
                    Content = q.Content,
                    Date = q.Date,
                    NumberOfAnswer = (q.Answers != null ? q.Answers.Count() : 0),
                    NumberOfLike = q.LikeQuestions.Count(),
                    userName = user != null ? user.Name : string.Empty,
                    userAvatar = user != null ? user.Avatar : string.Empty,
                    Answers = (q.HiddenAnswer == false) ? q.Answers : null
                };

                questionList.Add(questionDto);
            }

            return questionList;
        }
        public async Task<List<QuestionDTO>> QuestionAndAnswer(int subjectID)
        {
            var questionsAndAnswers = await _context.Questions
                        .Where(q => q.Topic.Subject.Id == subjectID)
                        .ToListAsync();
            var questionDTOs = await CreateListQuestionDTO(questionsAndAnswers);

            return questionDTOs;
        }
    }
}
