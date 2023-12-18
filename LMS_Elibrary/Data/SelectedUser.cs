using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class SelectedUser
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int ClassRoomNotificationId { get; set; }
        [ForeignKey(nameof(ClassRoomNotificationId))]
        [JsonIgnore]
        public ClassRoomNotification? ClassRoomNotifications { get; set; }

    }
}
