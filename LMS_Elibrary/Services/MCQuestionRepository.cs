using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static ServiceStack.Logging.TestLogger;

namespace LMS_Elibrary.Services
{
    public class MCQuestionRepository : IMCQuestionRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public MCQuestionRepository(ElibraryDbContext context, GetUser getUser)
        {
            _context = context;
            _getUser = getUser;
        }
        private List<MCQuestionsDTO> CreateListMCQuestionsDTO(List<MCQuestions> documents)
        {
            var _MCQuestionsDTO = documents.Select(sub => new MCQuestionsDTO
            {
                MCQuestionId = sub.Id,
                MCQuesKey = sub.MCKey,
                Level = sub.Level,
                Content = sub.Content,
                Creater = sub.Creater,
                CreateDate = sub.CreateDate,
                Updator = sub.Updator,
                LastUpdate = sub.LastUpdate,
            }).ToList();
            return _MCQuestionsDTO;
        }

        public async Task<MCQuestionsDTO> Add(CreateMCQuestionModel MCQuestion)
        {
            var isusser = await _getUser.user();
            var ques = await _context.MCQuestions.OrderByDescending(a => a.Id).ToListAsync();
            var f = ques.FirstOrDefault();
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.SubjectName == MCQuestion.Subject);
            var subjectGroup = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Name == MCQuestion.SubjectGroup);
            if (subject == null || subjectGroup == null)
            {
                return null;
            }
            if (f != null)
            {
                var quesKey = f.MCKey;
                string numberPart = quesKey.Substring(3);
                int number = int.Parse(numberPart) + 1;
                string formattedNumber = number.ToString().PadLeft(numberPart.Length, '0');
                string result = "MCH" + formattedNumber;

                var _newQ = new MCQuestions
                {
                    MCKey = result,
                    Level = MCQuestion.Level,
                    Content = MCQuestion.Content,
                    isSingleChoice = MCQuestion.isSingleChoice,
                    Creater = isusser.Name,
                    CreateDate = DateTime.Now,
                    Updator = isusser.Name,
                    LastUpdate = DateTime.Now,
                    SubjectId = subject.Id,
                    SubjectGroupId = subjectGroup.Id,
                    ExamId = MCQuestion.examId,
                };
                _context.MCQuestions.Add(_newQ);
                await _context.SaveChangesAsync();
                var newQuest = new MCQuestionsDTO
                {
                    MCQuestionId = _newQ.Id,
                    MCQuesKey = _newQ.MCKey,
                    Level = _newQ.Level,
                    Content = _newQ.Content,
                    Creater = _newQ.Creater,
                    CreateDate = _newQ.CreateDate,
                    Updator = _newQ.Updator,
                    LastUpdate = _newQ.LastUpdate,
                };
                return newQuest;
            }
            var newQ = new MCQuestions
            {
                MCKey = "MCH0001",
                Level = MCQuestion.Level,
                Content = MCQuestion.Content,
                isSingleChoice = MCQuestion.isSingleChoice,
                Creater = isusser.Name,
                CreateDate = DateTime.Now,
                Updator = isusser.Name,
                LastUpdate = DateTime.Now,
                SubjectId = subject.Id,
                SubjectGroupId = subjectGroup.Id
            };
            _context.MCQuestions.Add(newQ);
            await _context.SaveChangesAsync();
            var _newQuest = new MCQuestionsDTO
            {
                MCQuestionId = newQ.Id,
                MCQuesKey = newQ.MCKey,
                Level = newQ.Level,
                Content = newQ.Content,
                Creater = newQ.Creater,
                CreateDate = newQ.CreateDate,
                Updator = newQ.Updator,
                LastUpdate = newQ.LastUpdate,
            };
            return _newQuest;
        }

        public async Task<bool> Delete(int id)
        {
            var questionToDelete = await _context.MCQuestions.SingleOrDefaultAsync(a => a.Id == id);
            if (questionToDelete != null)
            {
                // Xóa các liên kết trong bảng QuestionAnswerMapping
                var mappingsToDelete = await _context.QuestionAnswerMapping
                    .Where(qam => qam.MCQuestionId == id).ToListAsync();

                _context.QuestionAnswerMapping.RemoveRange(mappingsToDelete);

                // xóa câu hỏi
                _context.MCQuestions.Remove(questionToDelete);

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<MCQuestionsDTO>> GetAll()
        {
            var result = await _context.MCQuestions.ToListAsync();
            var DTO = CreateListMCQuestionsDTO(result);
            return DTO;
        }

        public async Task<MCQuestionWithMCAnswersDTO> GetById(int id)
        {
            var result = await _context.MCQuestions.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return new MCQuestionWithMCAnswersDTO();
            }
            var answers = await _context.QuestionAnswerMapping
                 .Where(qam => qam.MCQuestionId == id)
                 .Select(qam => qam.MCAnswers)
                 .ToListAsync();
            var newDTO = new MCQuestionWithMCAnswersDTO
            {
                Id = result.Id,
                Content = result.Content,
                Answers = answers
            };
            return newDTO;

        }

        public async Task<List<string>> GetSubject()
        {
            var sub = await _context.Subjects.ToListAsync();
            var result = sub.Select(a => a.SubjectName).ToList();
            return result;
        }

        public async Task<List<string>> GetSubjectGroup()
        {
            var group = await _context.SubjectGroups.ToListAsync();
            var result = group.Select(a => a.Name).ToList();
            return result;
        }

        public async Task<bool> Update(int id, string questionContent, List<MCAnswers> mCAnswers)
        {
            var quest = await _context.MCQuestions.SingleOrDefaultAsync(a => a.Id == id);
            if (quest == null)
            {
                return false;
            }

            // 1. Xóa tất cả các câu trả lời cũ
            if (quest.QuestionAnswerMapping != null && quest.QuestionAnswerMapping.Any())
            {
                foreach (var mapping in quest.QuestionAnswerMapping.ToList())
                {
                    _context.MCAnswers.Remove(mapping.MCAnswers);
                    quest.QuestionAnswerMapping.Remove(mapping);
                }
            }

            // 2. Tạo lại các câu trả lời mới
            foreach (var newAnswer in mCAnswers)
            {
                var MCA = new MCAnswers
                {
                    Content = newAnswer.Content,
                    IsCorrect = newAnswer.IsCorrect,
                };
                _context.MCAnswers.Add(MCA);
                await _context.SaveChangesAsync();

                var newMapping = new QuestionAnswerMapping
                {
                    MCQuestionId = id,
                    MCAnswerId = MCA.id
                };
                _context.QuestionAnswerMapping.Add(newMapping);
            }
            quest.Content = questionContent;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<MCQuestionsDTO>> SortQuestion(string subjectGroup, string subject, List<string>? levels)
        {
            var _subjectGroup = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Name == subjectGroup);
            var _subject = await _context.Subjects.SingleOrDefaultAsync(a => a.SubjectName == subject);
            if (_subjectGroup == null || _subject == null)
            {
                return null;
            }
            var quest = await _context.MCQuestions
                                    .Where(a => a.SubjectGroupId == _subjectGroup.Id &&
                                                a.SubjectId == _subject.Id).ToListAsync();
            if (levels != null && levels.Any())
            {
                // Lọc theo các level được chọn
                var _quest = quest.Where(a => levels.Contains(a.Level)).ToList();
                var result = CreateListMCQuestionsDTO(_quest);
                return result;
            }
            var _result = CreateListMCQuestionsDTO(quest);
            return _result;
        }

        public async Task<List<MCQuestionsDTO>> Search(string searchString)
        {
            var result = await _context.MCQuestions.ToListAsync();
            var DTO = CreateListMCQuestionsDTO(result);
            var search = DTO.Where(a => a.MCQuesKey == searchString || a.Level == searchString || a.Creater == searchString || a.Updator == searchString).ToList();
            return search;
        }
        private async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public async Task<bool> SortQuestion(string subjectGroup, string subject, IFormFile formFile)
        {
            var newCreateMCQuestionModel = new CreateMCQuestionModel 
            {
                Level = "Thấp",
                Content = "",
                isSingleChoice = true,
                SubjectGroup = subjectGroup,
                Subject = subject
            };
            var a = await Add(newCreateMCQuestionModel);
            var _file = new MCQuestionFiles
            {
                FileName = formFile.FileName,
                FileData = await ConvertFormFileToByteArray(formFile),
                FileType = Path.GetExtension(formFile.FileName),
                FileSize = (int)formFile.Length,
                MCQuestionId = a.MCQuestionId,
            };
            _context.MCQuestionFiles.Add(_file);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
