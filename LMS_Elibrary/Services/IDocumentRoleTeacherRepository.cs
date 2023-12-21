using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IDocumentRoleTeacherRepository
    {
        public Task<PopupAddDocmentDTO> PopupAddLecture(int subId);
        public Task<List<Document>> AddLecture(AddDocRoleTeacherModel addDocRoleTeacherModel, List<IFormFile> files);
        public Task<PopupAddDocmentDTO> PopupAddResources(int subId);
        public Task<List<Document>> AddResources(AddDocRoleTeacherModel addDocRoleTeacherModel, List<IFormFile> files);
        public Task<List<DocumentRoleTeacherDTO>> GetAllBySubjectId(int subId);
        public Task<List<DocumentRoleTeacherDTO>> GetDocByStatus(string type);
        public Task<List<string>> GetStatus();
        public Task<List<DocumentRoleTeacherDTO>> Search(string searchString);
        public Task<List<DocumentRoleTeacherDTO>> SortDocument(string columnName, bool isAscending);
        public Task<Data.File> RreviewPopup(int docId);
        public Task<List<Data.File>> DownloadDocFile(List<int> ints);
        public Task<bool> Delete(List<int> id);

        public Task<string> CreateBy();
        public Task<SendDocModel> SearchUploadDocument(string searchString, string type);



    }
}
