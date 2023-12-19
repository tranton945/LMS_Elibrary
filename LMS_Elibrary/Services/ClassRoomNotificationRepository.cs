using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class ClassRoomNotificationRepository : IClassRoomNotificationRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassRoomNotificationRepository(ElibraryDbContext context, GetUser getUser, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _getUser = getUser;
            _userManager = userManager;
        }

        public async Task<ClassRoomNotificationDTO> Add(CreateClassRoomNotificationModel Notification)
        {
            var isusser = await _getUser.user();
            var classRooms = Notification.ClassRoom;
            if(Notification.ClassRoom.Contains("Tất cả các lớp"))
            {
                if(Notification.SubjectId == 0)
                {
                    return null;
                }
                classRooms = await _context.ClassRooms
                            .Where(s => s.SubjectId == Notification.SubjectId)
                            .Select(a => a.ClassRoomName)
                            .ToListAsync();
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var _notification = new ClassRoomNotification
                    {
                        Title = Notification.Title,
                        Content = Notification.Content,
                        Date = DateTime.Now,
                        CreatorId = isusser.Id,                  
                    };

                    _context.ClassRoomNotifications.Add(_notification);
                    await _context.SaveChangesAsync();

                    LinkNotificationToClassRooms(classRooms, _notification.Id);
                    if(Notification.StudentID != null && Notification.StudentID.Any())
                    {
                        LinkNotificationToSelectedUser(Notification.StudentID, _notification.Id);
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return new ClassRoomNotificationDTO
                    {
                        Id = _notification.Id,
                        Avatar = isusser.Avatar,
                        UserName = isusser.Name,
                        Date = _notification.Date,
                        Content = _notification.Content,
                        Title = _notification.Title
                    };
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        private async Task<List<ClassRoomNotificationDTO>> CreateClassRoomNotificationDTO(List<ClassRoomNotification>? notifications)
        {
            if (notifications == null || notifications.Count == 0)
            {
                return new List<ClassRoomNotificationDTO>();
            }
            List<ClassRoomNotificationDTO> answerDTOs = new List<ClassRoomNotificationDTO>();
            foreach (var notification in notifications)
            {
                var user = await _userManager.FindByIdAsync(notification.CreatorId);
                var _notification = new ClassRoomNotificationDTO
                {
                    Id = notification.Id,
                    Avatar = user != null ? user.Avatar : string.Empty,
                    UserName = user != null ? user.Name : string.Empty,
                    Date = notification.Date,
                    Content = notification.Content,
                    Title = notification.Title
                };
                answerDTOs.Add(_notification);
            }
            return answerDTOs;
        }
        private async void LinkNotificationToSelectedUser(List<string>? StudentIDs, int notificationId)
        {
            foreach (var StudentID in StudentIDs)
            {
                var user = await _userManager.FindByNameAsync(StudentID);
                if (user != null)
                {
                    var _selectedUser = new SelectedUser
                    {
                        ClassRoomNotificationId= notificationId,
                        StudentId = user.Id
                    };
                    _context.SelectedUsers.Add(_selectedUser);
                }
            }
        }
        private void LinkNotificationToClassRooms(List<string> classRoomNames, int notificationId)
        {
            foreach (var className in classRoomNames)
            {
                var classRoom = _context.ClassRooms.SingleOrDefault(c => c.ClassRoomName == className);
                if (classRoom != null)
                {
                    var notificationLink = new ClassRoomNotificationLink
                    {
                        ClassRoomId = classRoom.Id,
                        ClassRoomNotificationId = notificationId
                    };
                    _context.ClassRoomNotificationLinks.Add(notificationLink);
                }
            }
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.ClassRoomNotifications.SingleOrDefaultAsync(a => a.Id == id);
            if (result != null)
            {
                return false;
            }
            _context.ClassRoomNotifications.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClassRoomNotificationDTO>> GetAll()
        {
            var result = await _context.ClassRoomNotifications.ToListAsync();
            var DTOs = await CreateClassRoomNotificationDTO(result);
            return DTOs;
        }

        public async Task<ClassRoomNotificationDTO> GetById(int id)
        {
            var result = await _context.ClassRoomNotifications.SingleOrDefaultAsync(a => a.Id == id);
            var user = await _userManager.FindByIdAsync(result.CreatorId);
            var _notification = new ClassRoomNotificationDTO
            {
                Id = result.Id,
                Avatar = user != null ? user.Avatar : string.Empty,
                UserName = user != null ? user.Name : string.Empty,
                Date = result.Date,
                Content = result.Content,
                Title = result.Title
            };
            return _notification;
        }

        public async Task<List<string>> GetClassRoomInPopUp(int subjectId)
        {
            var result = await _context.ClassRooms
                .Where(f => f.SubjectId == subjectId)
                .Select(a => a.ClassRoomName).ToListAsync();
            result.Insert(0, "Tất cả các lớp");
            return result;
        }

        public async Task<string> SearchUser(string name)
        {
            var result = await _userManager.FindByNameAsync(name);

            return result.Name;
        }

        public async Task<bool> Update(ClassRoomNotification Notification, int id)
        {
            var result = await _context.ClassRoomNotifications.SingleOrDefaultAsync(a => a.Id == id);
            if(result == null)
            {
                return false;
            }
            result.Title = Notification.Title ?? result.Title;
            result.Content = Notification.Content ?? result.Content;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClassRoomNotificationDTO>> Search(string searchString)
        {
            var result = await _context.ClassRoomNotifications.ToListAsync();
            var DTOs = await CreateClassRoomNotificationDTO(result);
            var listNotification = DTOs.Where(a => a.UserName.Contains(searchString) || 
                                                    a.Title.Contains(searchString) || 
                                                    a.Content.Contains(searchString)).ToList();
            return listNotification;
        }

        public async Task<List<string>> GetClassRoomInScreenNotification(int subjectId)
        {
            var result = await _context.ClassRooms
                .Where(s => s.SubjectId == subjectId)
                .Select(a => a.ClassRoomName).ToListAsync();
            result.Insert(0, "Tùy chọn lớp");
            return result;
        }

        public async Task<List<ClassRoomNotificationDTO>> GetByClassRoom(string classRoomName)
        {
            var result = await _context.ClassRoomNotifications
                .Include(a => a.ClassRoomNotificationLinks)
                .ThenInclude(a => a.ClassRooms)
                .Where(a => a.ClassRoomNotificationLinks.Any(x => x.ClassRooms.ClassRoomName == classRoomName))
                .ToListAsync();
            var DTOs = await CreateClassRoomNotificationDTO(result);
            return DTOs;
        }

        public async Task<List<ClassRoomNotificationDTO>> GetBySubjectId(int subjectId)
        {
            var classroom = await _context.ClassRooms
                                        .Include(a => a.ClassRoomNotificationLinks)
                                        .ThenInclude(a => a.ClassRoomNotifications)
                                        .Where(a => a.SubjectId == subjectId)
                                        .ToListAsync();
            if(classroom == null || classroom.Count() == 0)
            {
                return null;
            }
            var notification = classroom.SelectMany(a => a.ClassRoomNotificationLinks.Select(x => x.ClassRoomNotifications)).Distinct().ToList();
            var DTOs = await CreateClassRoomNotificationDTO(notification);
            return DTOs;
        }
    }
}
