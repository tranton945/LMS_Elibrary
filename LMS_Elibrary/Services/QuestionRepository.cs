using LMS_Elibrary.Data;
using LMS_Elibrary.Migrations;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LMS_Elibrary.Services
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public QuestionRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        } 
        public async Task<Questions> Add(CreateQuestionModel q)
        {
            if(q.ClassRoom == "Tất cả các lớp")
            {
                return null;
            }
            var isusser = await _getUser.user();
            var classRoom = await _context.ClassRooms.SingleOrDefaultAsync(a => a.ClassRoomName == q.ClassRoom);
            var topic = await _context.Topics.SingleOrDefaultAsync(a => a.TopicName == q.Topic);
            var lecture = await _context.Lectures
                .Include(a => a.Topic)
                .Where(a => a.Topic.Id == topic.Id)
                .SingleOrDefaultAsync(a => a.Title == q.Lecture);
            var question = new Questions
            {
                Title= q.Title,
                Contain = q.Contain,
                Like = false,
                Date = DateTime.Now,
                HiddenAnswer = false,
                Answered = false,
                UserId = isusser.Id,
                ClassRoomId = classRoom.Id,
                TopicId = (topic != null) ? topic.Id : null,
                LectureId = (lecture != null) ? lecture.Id : null
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Questions.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            }
            _context.Answers.RemoveRange(result.Answers);
            _context.Questions.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionDTO>> GetAll()
        {
            var isusser = await _getUser.user();
            var result = await _context.Questions
                                    .Include(a => a.Answers)
                                    .Include(a => a.LikeQuestions)
                                    .ToListAsync();
            var SubjectTeacher = CreateListQuestionDTO(result, isusser);

            return SubjectTeacher;
        }

        public async Task<List<string>> GetAllTopic()
        {
            var result = await _context.Topics.ToListAsync();
            var topic = result.Select(a => a.TopicName).ToList();
            topic.Insert(0, "Tùy chọn chủ đề");
            return topic;
        }
        public async Task<List<string>> GetLectureByTopic(string topic)
        {
            if(topic == "Tùy chọn chủ đề")
            {
                return null;
            }
            var result = await _context.Lectures
                .Include(a => a.Topic)
                .Where(a => a.Topic.TopicName == topic).ToListAsync();
            var lecture = result.Select(a => a.Title).ToList();
            lecture.Insert(0, "Tùy chọn bài giảng");
            return lecture;
        }

        public async Task<QuestionDTO> GetById(int id)
        {
            var isusser = await _getUser.user();
            var result = await _context.Questions
                            .Include(a => a.Answers)
                            .Include(a => a.LikeQuestions)
                            .Select(q => new QuestionDTO
                            {
                                QuestionId = q.Id,
                                Title = q.Title,
                                Contain = q.Contain,
                                Date = q.Date,
                                NumberOfAnswer = (q.Answers != null ? q.Answers.Count() : 0),
                                NumberOfLike = q.LikeQuestions.Count(),
                                userName = isusser.UserName,
                                userAvatar = isusser.Avatar,
                                Answers = (q.HiddenAnswer == false) ? q.Answers : null
                            })
                            .SingleOrDefaultAsync(a => a.QuestionId == id);

            if(result == null)
            {
                return new QuestionDTO();
            }
            return result;
        }

        private List<QuestionDTO> CreateListQuestionDTO(List<Questions>? ques, ApplicationUser user)
        {
            if (ques == null || ques.Count == 0)
            {
                return new List<QuestionDTO>();
            }
            var question = ques.Select(q => new QuestionDTO
            {
                QuestionId = q.Id,
                Title = q.Title,
                Contain = q.Contain,
                Date = q.Date,
                NumberOfAnswer = (q.Answers != null ? q.Answers.Count() : 0),
                NumberOfLike = q.LikeQuestions.Count(),
                userName = user.UserName,
                userAvatar = user.Avatar,
                Answers = (q.HiddenAnswer == false) ? q.Answers : null
            }).ToList();
            return question;
        }

        public Task<bool> Update(Questions q, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllClassRoom()
        {
            var result = await _context.ClassRooms.ToListAsync();
            var classRoom = result.Select(a => a.ClassRoomName).ToList();
            classRoom.Insert(0, "Tùy chọn lớp");
            return classRoom;
        }


        public async Task<List<QuestionDTO>> GetByClassRoom(string classRoom)
        {
            if(classRoom.ToLower() == "tùy chọn lớp")
            {
                return await GetAll();
            }
            var isusser = await _getUser.user();
            var classroom = await _context.ClassRooms.SingleOrDefaultAsync(a => a.ClassRoomName == classRoom);
            var result = await _context.Questions
                                    .Include(a => a.Answers)
                                    .Include(a => a.LikeQuestions)
                                    .Where(a => a.ClassRoomId == classroom.Id)
                                    .ToListAsync();
            var _question = CreateListQuestionDTO(result, isusser);

            return _question;
        }

        public async Task<List<string>> GetAllLecture()
        {
            var lectures = await _context.Lectures.ToListAsync();
            var lec = lectures.Select(a => a.Title).Distinct().ToList();
            lec.Insert(0, "Tất cả bài giảng");
            return lec;

        }
        public async Task<List<QuestionDTO>> GetByLecture(string lecture)
        {
            if (lecture.ToLower() == "tất cả bài giảng")
            {
                return await GetAll();
            }
            var isusser = await _getUser.user();
            var _lecture = await _context.Lectures
                                        .Where(a => a.Title == lecture)
                                        .Select(a => a.Id)
                                        .ToListAsync();
            var result = await _context.Questions
                                    .Include(a => a.Answers)
                                    .Include(a => a.LikeQuestions)
                                    .Where(a => _lecture.Contains(a.LectureId.Value))
                                    .ToListAsync();
            var _question = CreateListQuestionDTO(result, isusser);

            return _question;
        }

        public async Task<List<QuestionDTO>> GetByDate()
        {
            var isusser = await _getUser.user();
            var result = await _context.Questions
                                    .Include(a => a.Answers)
                                    .Include(a => a.LikeQuestions)
                                    .OrderByDescending(a => a.Date)
                                    .ToListAsync();
            var _question = CreateListQuestionDTO(result, isusser);

            return _question;
        }

        public async Task<List<QuestionDTO>> GetByAnswered(bool answered)
        {
            var isusser = await _getUser.user();
            var result = await _context.Questions
                                    .Include(a => a.Answers)
                                    .Include(a => a.LikeQuestions)
                                    .Where(a => a.Answered == answered)
                                    .ToListAsync();
            var _question = CreateListQuestionDTO(result, isusser);

            return _question;
        }

        public async Task<List<QuestionDTO>> GetMyQuestions()
        {
            var isusser = await _getUser.user();
            var result = await _context.Questions
                                    .Include(a => a.Answers)
                                    .Include(a => a.LikeQuestions)
                                    .Where(a => a.UserId == isusser.Id)
                                    .ToListAsync();
            var _question = CreateListQuestionDTO(result, isusser);

            return _question;
        }

        public async Task<bool> LikeQuestion(int questionId)
        {
            var isusser = await _getUser.user();
            var result = await _context.Questions.SingleOrDefaultAsync(a => a.Id== questionId);
            if(result == null)
            {
                return false;
            }
            var likeQuestion = new LikeQuestions
            {
                UserId = isusser.Id,
                QuestionId = questionId
            };
            result.Like = true;
            _context.LikeQuestions.Add(likeQuestion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionDTO>> GetMyLike()
        {
            var isusser = await _getUser.user();
            var _likeQuestions = await _context.LikeQuestions
                .Where(a => a.UserId == isusser.Id)
                .Select(a => a.Id)
                .ToListAsync();
            var result = await _context.Questions
                        .Include(a => a.Answers)
                        .Include(a => a.LikeQuestions)
                        .Where(a => _likeQuestions.Contains(a.Id))
                        .ToListAsync();
            var _question = CreateListQuestionDTO(result, isusser);

            return _question;
        }

        public async Task<bool> HidentAnswer(int questionId, bool hidentAnswer)
        {
            var isusser = await _getUser.user();
            var question = await _context.Questions.SingleOrDefaultAsync(a => a.Id == questionId);
            if(question == null)
            {
                return false;
            }
            question.HiddenAnswer = hidentAnswer;
            return true;
        }

        public Task<List<string>> GetQuestionType()
        {
            var d = new List<string>
            {
                "Câu hỏi gần nhất",
                "Câu hỏi chưa trả lời",
                "Câu hỏi đã trả lời"
            };
            return Task.FromResult(d);
        }

        public async Task<List<QuestionDTO>> GetQuestionByType(string type)
        {
            if(type == "Câu hỏi gần nhất")
            {
                var result = await GetByDate();
                return result;
            }
            if(type == "Câu hỏi chưa trả lời")
            {
                var result = await GetByAnswered(false);
                return result;
            }
            if(type == "Câu hỏi đã trả lời")
            {
                var result = await GetByAnswered(true);
                return result;
            }
            
            return await GetAll();
        }

        public Task<List<string>> GetQuestionFilter()
        {
            var d = new List<string>
            {
                "Câu hỏi tôi hỏi",
                "Câu hỏi tôi thích"
            };
            return Task.FromResult(d);
        }

        public async Task<List<QuestionDTO>> GetQuestionByFilter(string filter)
        {
            if (filter == "Câu hỏi tôi hỏi")
            {
                var result = await GetMyQuestions();
                return result;
            }
            if (filter == "Câu hỏi tôi thích")
            {
                var result = await GetMyLike();
                return result;
            }
            return await GetAll();
        }
    }
}
