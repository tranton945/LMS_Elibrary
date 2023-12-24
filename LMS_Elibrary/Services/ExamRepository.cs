using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class ExamRepository : IExamRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly IMCQuestionRepository _mCQuestion;
        private readonly IMCAnswerRepository _mCAnswer;

        public ExamRepository(ElibraryDbContext context, GetUser getUser, IMCQuestionRepository mCQuestion, IMCAnswerRepository mCAnswer) 
        {
            _context = context;
            _getUser = getUser;
            _mCQuestion = mCQuestion;
            _mCAnswer = mCAnswer;
        }
        public async Task<bool> ChangeName(int id, string newName)
        {
            var exam = await _context.Exams.SingleOrDefaultAsync(a => a.Id == id);
            if (exam == null)
            {
                return false;
            }
            exam.ExamName = newName ?? exam.ExamName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ExamDTO> CreateExamES(ExamESModel model)
        {
            var isusser = await _getUser.user();
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.SubjectName == model.Subject);
            var subjectGroup = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Name == model.SubjectGroup);
            if (subject == null || subjectGroup == null)
            {
                return null;
            }
            var exam = new Exams
            {
                Type = "Tự luận",
                ExamName = model.Name,
                Format = ".CSV",
                Duration = model.Duration,
                Creator = isusser.Name,
                Date = DateTime.Now,
                Status = (model.isDraft = true) ? "Lưu nháp" : "Chờ phê duyệt",
                Approved = false,
                isDraft = model.isDraft,
                SubjectGroupId = subjectGroup.Id,
                SubjectId = subject.Id
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            foreach(var _model in model.esModel)
            {
                var es = new EssayQuestions
                {
                    Answer= _model.Answer,
                    UploadFile = _model.UploadFile,
                    ExamId = exam.Id
                };
                _context.EssayQuestions.Add(es);
            }
            await _context.SaveChangesAsync();
            var DTO = new ExamDTO
            {
                ExamId = exam.Id,
                Name = exam.ExamName,
                Type = exam.Type,
                Duration = exam.Duration.ToString(),
                Creator = exam.Creator,
                CreateDate = DateTime.Now,
                Status = exam.Status,
            };
            return DTO;
        }

        public async Task<ExamDTO> CreateExamMC(ExamMCModel model)
        {
            var isusser = await _getUser.user();
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.SubjectName == model.Subject);
            var subjectGroup = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Name == model.SubjectGroup);
            if (subject == null || subjectGroup == null)
            {
                return null;
            }
            var exam = new Exams
            {
                Type = "Trắc nghiệm",
                ExamName= model.Name,
                Format = ".CSV",
                Duration = model.Duration,
                Creator = isusser.Name,
                Date = DateTime.Now,
                Status = (model.isDraft == true) ? "Lưu nháp" : "Chờ phê duyệt",
                Approved = false,
                isDraft = model.isDraft,
                SubjectGroupId = subjectGroup.Id,
                SubjectId = subject.Id
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            foreach(var _model in model.QuestionAndAnswerModel)
            {
                var mcQuestionModel = new CreateMCQuestionModel
                {
                    Level = "Thấp",
                    Content = _model.Content,
                    isSingleChoice = true,
                    SubjectGroup = model.SubjectGroup,
                    Subject = model.Subject,
                    examId = exam.Id
                };
                var mcQuestion = await _mCQuestion.Add(mcQuestionModel);
                var _answer = await _mCAnswer.Add(_model.MCAnswers, mcQuestion.MCQuestionId);
            }
            await _context.SaveChangesAsync();
            var DTO = new ExamDTO
            {
                ExamId= exam.Id,
                Name = exam.ExamName,
                Type = exam.Type,
                Duration =  exam.Duration.ToString(),
                Creator = exam.Creator,
                CreateDate = DateTime.Now,
                Status = exam.Status,
            };
            return DTO;
        }

        public async Task<List<ExamDTO>> CreateFromBank(CreateExamFromBank model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            var exam = await _context.Exams.SingleOrDefaultAsync(a => a.Id == id);
            if (exam == null)
            {
                return false;
            }
            var MCquestions = await _context.MCQuestions.Where(a => a.ExamId == id).ToListAsync();
            foreach(var mcQuestion in MCquestions) 
            {
                mcQuestion.ExamId = null;
            }
            await _context.SaveChangesAsync();

            var relatedEssays = _context.EssayQuestions.Where(e => e.ExamId == id).ToList();
            foreach (var essay in relatedEssays)
            {
                // Tìm và xóa tất cả các essayFile liên quan
                var relatedEssayFiles = _context.EQuestAnswerFiles.Where(file => file.Id == essay.Id).ToList();
                _context.EQuestAnswerFiles.RemoveRange(relatedEssayFiles);

                // Xóa essay
                _context.EssayQuestions.Remove(essay);
            }
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> Download(int id, string newName)
        {
            throw new NotImplementedException();
        }
        private List<ExamDTO> CreateListExamDTO(List<Exams> ex)
        {
            var ExamDTO = ex.Select(exam => new ExamDTO
            {
                ExamId = exam.Id,
                Name = exam.ExamName,
                Type = exam.Type,
                Duration = exam.Duration.ToString(),
                Creator = exam.Creator,
                CreateDate = DateTime.Now,
                Status = exam.Status,
            }).ToList();
            return ExamDTO;
        }
        public async Task<List<ExamDTO>> GetAll()
        {
            var exam = await _context.Exams.OrderByDescending(a => a.Date).ToListAsync();
            var DTO = CreateListExamDTO(exam);
            return DTO;
        }

        public async Task<MCExamDetailDTO> GetById(int id)
        {
            var exam = await _context.Exams
                .SingleOrDefaultAsync(a => a.Id == id);
            var mcQuest = await _context.MCQuestions.Where(e => e.ExamId == id).ToListAsync();
            var questionIds = mcQuest.Select(a => a.Id).ToList();
            var answers = _context.QuestionAnswerMapping
                .Where(mapping => questionIds.Contains(mapping.MCQuestionId))
                .Select(mapping => mapping.MCAnswers)
                .ToList();
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.Id == exam.SubjectId);
            var subjectGroup = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Id == exam.SubjectGroupId);
            var correctAnswer = answers.Where(a => a.IsCorrect == true).Select(a => a.Content).ToList();
            var newMCExamDetailDTO = new MCExamDetailDTO
            {
                SubjectName = subject.SubjectName,
                ExamName = exam.ExamName,
                Duration = exam.Duration.ToString(),
                Type = exam.Type,
                QuestionWithAnswers = new List<QuestionWithAnswers>(),
                CorrectAnswer = correctAnswer
            };
            foreach (var a in mcQuest)
            {
                var _questionWithAnswers = new QuestionWithAnswers
                {
                    QuestionContent = a.Content,
                    Answers = answers.Select(c => c.Content).ToList()
                };
                newMCExamDetailDTO.QuestionWithAnswers.Add(_questionWithAnswers);
            };
        

            if (exam == null)
            {
                return null;
            }
            var DTO = new ExamDTO
            {
                ExamId = exam.Id,
                Name = exam.ExamName,
                Type = exam.Type,
                Duration = exam.Duration.ToString(),
                Creator = exam.Creator,
                CreateDate = DateTime.Now,
                Status = exam.Status,
            };
            return newMCExamDetailDTO;
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

        public async Task<List<ExamDTO>> Search(string searchString)
        {
            var exam = await _context.Exams.ToListAsync();
            var DTO = CreateListExamDTO(exam);
            var search = DTO.Where(a => a.Name.Contains(searchString) || a.Creator.Contains(searchString)).ToList();
            return search;
        }

        public async Task<bool> SendApprove(int id)
        {
            var exam = await _context.Exams.SingleOrDefaultAsync(a => a.Id == id);
            if (exam == null)
            {
                return false;
            }
            exam.isDraft = false;
            exam.Approved= false;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string> GetUser()
        {
            var user = await _getUser.user();
            return user.Name;
        }
        public async Task<bool> Upload(string? fileName, string SubjectGroup, string GetSubject, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExamDTO>> SortBySubjectGroup(string searchString)
        {
            var subjectGroup = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Name == searchString);
            if (subjectGroup == null)
            {
                return new List<ExamDTO>();
            }
            var exam = await _context.Exams.Where(sub => sub.SubjectGroupId == subjectGroup.Id).ToListAsync();
            var DTO = CreateListExamDTO(exam);

            return DTO;
        }

        public async Task<List<ExamDTO>> SortBySubject(string searchString)
        {
            var subject = await _context.Subjects.SingleOrDefaultAsync(a => a.SubjectName == searchString);
            if (subject == null)
            {
                return new List<ExamDTO>();
            }
            var exam = await _context.Exams.Where(sub => sub.SubjectId == subject.Id).ToListAsync();
            var DTO = CreateListExamDTO(exam);
            return DTO;
        }

        private List<ExamDTOLeadership> CreateListExamDTOLeadership(List<Exams> ex)
        {
            var ExamDTO = ex.Select(exam => new ExamDTOLeadership
            {
                ExamId = exam.Id,
                Name = exam.ExamName,
                Subject = exam.Subject.SubjectName,
                Type = exam.Type,
                Duration = exam.Duration.ToString(),
                Creator = exam.Creator,
                CreateDate = DateTime.Now,
                Status = exam.Status,
            }).ToList();
            return ExamDTO;
        }

        public async Task<List<ExamDTOLeadership>> GetAllLeaderShip()
        {
            var resule = await _context.Exams.Include(s => s.Subject).ToListAsync();
            var DTO = CreateListExamDTOLeadership(resule);
            return DTO;
        }

        public async Task<List<string>> GetTeacher()
        {
            var resule = await _context.Exams.Select(a => a.Creator).Distinct().ToListAsync();
            resule.Insert(0, "Tất cả");
            return resule;
        }

        public async Task<List<string>> GetStatus()
        {
            var status = new List<string>
            {
                "Tất cả tình trạng",
                "Chờ phê duyệt",
                "Đã phê duyệt"
            };
            return await Task.FromResult(status);
        }

        public async Task<List<ExamDTOLeadership>> SearchLeaderShip(string searchString)
        {
            var resule = await _context.Exams.Include(s => s.Subject).ToListAsync();
            var DTO = CreateListExamDTOLeadership(resule);
            var s = DTO.Where(a => a.Name.Contains(searchString) || a.Subject.Contains(searchString) || a.Creator.Contains(searchString)).ToList();
            return s;
        }

        public async Task<bool> Approve(int id)
        {
            var resule = await _context.Exams.SingleOrDefaultAsync(a => a.Id == id);
            if(resule == null)
            {
                return false;
            }
            resule.isDraft = false;
            resule.Approved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RefuseApproval(int id)
        {
            var resule = await _context.Exams.SingleOrDefaultAsync(a => a.Id == id);
            if (resule == null)
            {
                return false;
            }
            resule.isDraft = false;
            resule.Approved = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ExamDTOLeadership>> SortByTeacherLeadership(string searchString)
        {
            var resule = await _context.Exams.Include(s => s.Subject).ToListAsync();
            var DTO = CreateListExamDTOLeadership(resule);
            var s = DTO.Where(a => a.Creator == searchString).ToList();
            return s;
        }

        public async Task<List<ExamDTOLeadership>> SortBySubjectLeadership(string searchString)
        {
            var resule = await _context.Exams.Include(s => s.Subject).ToListAsync();
            var DTO = CreateListExamDTOLeadership(resule);
            var s = DTO.Where(a => a.Subject == searchString).ToList();
            return s;
        }

        public async Task<List<ExamDTOLeadership>> SortByStatusLeadership(string searchString)
        {
            var resule = await _context.Exams.Include(s => s.Subject).ToListAsync();
            var DTO = CreateListExamDTOLeadership(resule);
            var s = DTO.Where(a => a.Status ==  searchString).ToList();
            return s;
        }
    }
}
