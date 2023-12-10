using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IDocumentRepository
    {
        public Task<List<Document>> GetAll();
        public Task<Document> GetById(int id);
        public Task<DocumentInfo> GetDocInforById(int id);
        public Task<List<DocumentInfo>> GetAllDocInfor();
        public Task<Document> Add(Document document, IFormFile files);
        public Task<bool> Update(Document document, int id);
        public Task<bool> ApproveDoc(int id);
        public Task<bool> DoNotApproveDoc(DoNotApproveDocument doNotApproveDocument);
        public Task<DoNotApproveDocument> PopUpDoNotApproveDoc(int docId);
        public Task<bool> Delete(int id);

        public Task<List<Document>> Search(string searchString);
        public Task<List<Document>> GetDocByName(string _docName);
        public Task<List<string>> GetAllDocName();
        public Task<List<Document>> GetDocByTeacher(string teacher);
        public Task<List<string>> GetAllTeacher();
        public Task<List<Document>> GetDocByApprove(string type);
        public Task<List<string>> GetApproveType();
        public Task<List<Document>> GetDocBySchoolYear(string schoolYear);
        public Task<List<string>> GetSchoolYear();
        public Task<List<Data.File>> DownloadDocFile(List<int> ints);
    }
}
