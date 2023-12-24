using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface INotificationRepository
    {
        public Task<List<Notification>> GetAll();
        public Task<List<Notification>> Search(string searchString);
        public Task<List<Notification>> Sort(string type);
        public Task<Notification> GetById(int id);
        public Task<Notification> Add(Notification notification);
        public Task<bool> IsRead(List<int> id);
        public Task<bool> Delete(List<int> id);
    }
}
