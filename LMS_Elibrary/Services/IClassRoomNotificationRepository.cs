using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IClassRoomNotificationRepository
    {
        public Task<List<ClassRoomNotificationDTO>> GetAll();
        public Task<ClassRoomNotificationDTO> GetById(int id);
        public Task<List<string>> GetClassRoomInPopUp(int subjectId);
        public Task<List<ClassRoomNotificationDTO>> Search(string searchString);
        public Task<List<string>> GetClassRoomInScreenNotification(int subjectId);
        public Task<List<ClassRoomNotificationDTO>> GetByClassRoom(string classRoomName);
        public Task<string> SearchUser(string name);
        public Task<ClassRoomNotificationDTO> Add(CreateClassRoomNotificationModel Notification);
        public Task<bool> Update(ClassRoomNotification Notification, int id);
        public Task<bool> Delete(int id);
    }
}
