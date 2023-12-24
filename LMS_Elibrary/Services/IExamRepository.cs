using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IExamRepository
    {
        public Task<ExamDTO> CreateExamMC(ExamMCModel model);
        public Task<ExamDTO> CreateExamES(ExamESModel model);
        public Task<List<ExamDTO>> CreateFromBank(CreateExamFromBank model);
        public Task<List<ExamDTO>> GetAll();
        public Task<MCExamDetailDTO> GetById(int id);
        public Task<bool> ChangeName(int id, string newName);
        public Task<bool> Download(int id, string newName);
        public Task<bool> Upload(string? fileName, string SubjectGroup, string GetSubject, IFormFile file);
        public Task<bool> SendApprove(int id);
        public Task<bool> Delete(int id);
        public Task<List<string>> GetSubjectGroup();
        public Task<List<string>> GetSubject();
        public Task<string> GetUser();
        public Task<List<ExamDTO>> Search(string searchString);
        public Task<List<ExamDTO>> SortBySubjectGroup(string searchString);
        public Task<List<ExamDTO>> SortBySubject(string searchString);

        public Task<List<ExamDTOLeadership>> GetAllLeaderShip();
        public Task<List<string>> GetTeacher();
        public Task<List<string>> GetStatus();
        public Task<List<ExamDTOLeadership>> SearchLeaderShip(string searchString);
        public Task<bool> Approve(int id);
        public Task<bool> RefuseApproval(int id);
        public Task<List<ExamDTOLeadership>> SortByTeacherLeadership(string searchString);
        public Task<List<ExamDTOLeadership>> SortBySubjectLeadership(string searchString);
        public Task<List<ExamDTOLeadership>> SortByStatusLeadership(string searchString);

    }
}
