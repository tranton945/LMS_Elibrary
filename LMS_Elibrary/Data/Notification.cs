using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string? Sender { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }

    }
}
