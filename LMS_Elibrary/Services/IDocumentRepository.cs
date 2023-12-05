using LMS_Elibrary.Data;

namespace LMS_Elibrary.Services
{
    public interface IDocumentRepository
    {
        public Task<List<Document>> GetAll();
        public Task<Document> GetById(int id);
        public Task<DocumentInfo> GetDocInforById(int id);
        public Task<List<DocumentInfo>> GetAllDocInfor();
        public Task<Document> Add(Document document);
        public Task<bool> Update(Document document, int id);
        public Task<bool> Delete(int id);
    }
}
