using LMS_Elibrary.Data;

namespace LMS_Elibrary.Services
{
    public interface ISubjectRepository
    {
        public Task<List<Subject>> GetAll();
        public Task<Subject> GetById(int id);
        public Task<List<Subject>> Search(string searchString);
        public Task<List<Subject>> GetSubjectByName(string _subjectName);
        public Task<List<string>> GetAllSubjectName();
        public Task<List<Subject>> GetSubjectByTeacher(string teacher);
        public Task<List<string>> GetAllTeacher(List<AccountWithRolesDto> listAccount);
        //public Task<List<Subject>> GetSubjectByApprove(int id);
        public Task<Subject> Add(Subject subject);
        public Task<bool> Update(Subject subject, int id);
        public Task<bool> Delete(int id);
        public Task<List<string>> GetSchoolYear();
        public Task<List<Subject>> GetSubjectBySchoolYear(string schoolYear);
    }
}
