using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class SubjectNotificationRepository : ISubjectNotificationRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public SubjectNotificationRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }
        public async Task<SubjectNotification> Add(SubjectNotification subjectNotification, int subjectId)
        {
            var isuser = await _getUser.user();
            var notification = new SubjectNotification
            {
                Title= subjectNotification.Title,
                Content = subjectNotification.Content,
                CreatorId = isuser.Id,
                Date = DateTime.Now,
                SubjectId = subjectId,
            };
            return notification;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubjectNotification>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SubjectNotification> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubjectNotification>> GetBySubjectId(int id)
        {
            var resule = await _context.SubjectNotifications.Where(a => a.SubjectId == id).ToListAsync();
            return resule;
        }

        public Task<bool> Update(SubjectNotification subjectNotification, int id)
        {
            throw new NotImplementedException();
        }
    }
}
