using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class UserNotification
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool isRead { get; set; }

        public int NotificationID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(NotificationID))]
        public Notification? Notification { get; set; }

    }
}
