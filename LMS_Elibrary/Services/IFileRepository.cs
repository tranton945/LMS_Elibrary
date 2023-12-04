using LMS_Elibrary.Data;

namespace LMS_Elibrary.Services
{
    public interface IFileRepository
    {
        public Task<List<Data.File>> GetAll();
        public Task<Data.File> GetById(int id);
        public Task<Data.File> Add(IFormFile file, int docId);
        public Task<bool> Update(IFormFile file, int docId, int id);
        public Task<bool> Delete(int id);
    }
}
