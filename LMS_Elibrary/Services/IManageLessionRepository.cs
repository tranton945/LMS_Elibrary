using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IManageLessionRepository
    {
        //manage Lession
        public Task<List<ManageDocRoleTeacherDTO>> ManageGetAllLession();
        public Task<List<string>> ManageGetSubjectName();
        public Task<List<string>> ManageGetSubjectNamePopup();
        public Task<List<ManageDocRoleTeacherDTO>> ManageGetBySubjectName(string SubjectName);
        public Task<List<ManageDocRoleTeacherDTO>> ManageSearch(string searchString);
        public Task<bool> ManageUpload(List<IFormFile> files, string subjectName);
        public Task<List<Data.File>> ManageDownload(List<int> DocIds);
        public Task<Data.File> ManagePreview(int docId);
        public Task<bool> ManageChangeName(int docId, string newName);
        public Task<bool> ManageAddToSubject(int docId, string topic,string lectureName, List<string>? classRoomNames);
        public Task<bool> ManageDeleteDoc(List<int> DocIds);
        public Task<List<string>> ManageGetTopicBySubjectName(string subjectName);
    }
}
