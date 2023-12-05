using LMS_Elibrary.Data;

namespace LMS_Elibrary.Services
{
    public interface ITopicRepository
    {
        public Task<List<Topic>> GetAll();
        public Task<Topic> GetById(int id);
        public Task<List<Topic>> GetBySubjectId(int id);
        public Task<Topic> Add(Topic topic);
        public Task<bool> Update(Topic topic, int id);
        public Task<bool> Delete(int id);
    }
}
