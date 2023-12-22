using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IManageResourceRepository
    {
        //manage Resource
        public Task<List<ManageDocRoleTeacherDTO>> ManageGetAllResource();
        public Task<List<string>> ManageGetSubjectName();
        public Task<List<string>> ManageGetSubjectNamePopup();
        public Task<List<ManageDocRoleTeacherDTO>> ManageGetBySubjectName(string SubjectName);
        public Task<List<ManageDocRoleTeacherDTO>> ManageSearch(string searchString);
        public Task<bool> ManageUpload(List<IFormFile> files, string subjectName);
        public Task<List<Data.File>> ManageDownload(List<int> DocIds);
        public Task<Data.File> ManagePreview(int fileId);
        public Task<bool> ManageChangeName(int fileId, string newName);
        public Task<bool> ManageAddToSubject(string lectureName, List<string>? classRoomNames);
        public Task<bool> ManageDeleteFile(List<int> DocIds);
        public Task<List<string>> ManageGetTopicBySubjectName(string subjectName);
        public Task<List<string>> ManageGetLectureByTopicName(string topicName);
    }
}
