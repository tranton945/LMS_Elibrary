using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface ISubjectGroupRepository
    {
        public Task<List<SubjectGroup>> GetAll();
        public Task<SubjectGroup> GetById(int id);
        public Task<List<SubjectGroup>> GetBySubjectGrouptId(int id);
        public Task<SubjectGroup> Add(SubjectGroup subjectGroup);
        public Task<bool> Update(SubjectGroup subjectGroup, int id);
        public Task<bool> Delete(int id);
    }
}
