using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace LMS_Elibrary.Services
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationRepository(ElibraryDbContext context, GetUser getUser, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _getUser = getUser;
            _userManager = userManager;
        }

        public async Task<Notification> Add(Notification notification)
        {
            var isuser = await _getUser.user();
            var newNo = new Notification
            {
                Sender = isuser.Name,
                Content = notification.Content,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            var users = await _userManager.Users.ToListAsync();
            var userNotifications = users.Select(user => new UserNotification
            {
                UserId = user.Id,
                NotificationID = newNo.Id,
                isRead = false
            });
            _context.UserNotifications.AddRange(userNotifications);
            await _context.SaveChangesAsync();

            return newNo;
        }

        public async Task<bool> Delete(List<int> id)
        {
            var userNotifications = await _context.UserNotifications
                        .Where(un => id.Contains(un.NotificationID))
                        .ToListAsync();

            _context.UserNotifications.RemoveRange(userNotifications);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Notification>> GetAll()
        {
            var result = await _context.Notifications.ToListAsync();
            return result;
        }

        public async Task<Notification> GetById(int id)
        {
            var result = await _context.Notifications.SingleOrDefaultAsync(un => un.Id == id);
            if(result == null)
            {
                return new Notification();
            }
            return result;
        }

        public async Task<bool> IsRead(List<int> id)
        {
            var userNotifications = await _context.UserNotifications
                .Where(un => id.Contains(un.NotificationID) && un.isRead == false)
                .ToListAsync();

            foreach (var notification in userNotifications)
            {
                notification.isRead = true;
            }

            _context.UserNotifications.UpdateRange(userNotifications);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Notification>> Search(string searchString)
        {
            var result = await _context.Notifications.Where(a => a.Content.Contains(searchString)).ToListAsync();
            return result;
        }

        public async Task<List<Notification>> Sort(string type)
        {
            var isuser = await _getUser.user();
            var result = await _context.Notifications.Include(a => a.UserNotifications).ToListAsync();
            if(type.ToLower() == "tất cả")
            {
                return result;
            }
            if(type.ToLower() == "chưa đọc")
            {
                var unreadNotifications = await _context.UserNotifications
                        .Where(un => un.UserId == isuser.Id && un.isRead == false)
                        .Select(un => un.Notification)
                        .ToListAsync();
                return unreadNotifications;
            }
            if(type.ToLower() == "đã đọc")
            {
                var readNotifications = await _context.UserNotifications
                            .Where(un => un.UserId == isuser.Id && un.isRead == true)
                            .Select(un => un.Notification)
                            .ToListAsync();
                return readNotifications;
            }
            return result;
        }
    }
}
