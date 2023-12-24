using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class ClassRoomNotification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Content { get; set; }
        public string CreatorId { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ClassRoomNotificationLink>? ClassRoomNotificationLinks { get; set; }
        public ICollection<SelectedUser>? SelectedUsers { get; set; }
    }
}
