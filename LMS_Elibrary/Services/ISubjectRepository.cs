using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface ISubjectRepository
    {
        public Task<List<SubjectLeadershipDTO>> GetAll();
        public Task<Subject> GetById(int id);
        public Task<List<SubjectLeadershipDTO>> Search(string searchString);
        public Task<List<SubjectLeadershipDTO>> GetSubjectByName(string _subjectName);
        public Task<List<string>> GetAllSubjectName();
        public Task<List<SubjectLeadershipDTO>> GetSubjectByTeacher(string teacher);
        public Task<List<string>> GetAllTeacher();
        public Task<List<SubjectLeadershipDTO>> GetSubjectByApproveDocType(string Type);
        public Task<List<string>> GetApproveDocType();
        public Task<List<string>> GetSchoolYear();
        public Task<List<SubjectLeadershipDTO>> GetSubjectBySchoolYear(string schoolYear);
        public Task<Subject> Add(Subject subject);
        public Task<bool> Update(Subject subject, int id);
        public Task<bool> Delete(int id);

        public Task UpdateApproveDoc(int Id);
        public Task<bool> AddTeacherToSubject(int subjectId, string name);
        public Task<bool> AddStudentToSubject(int subjectId, string name);



    }
}
