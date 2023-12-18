using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class ClassRoomNotificationLink
    {
        [Key] 
        public int Id { get; set; }
        public int ClassRoomId { get; set; }
        [ForeignKey(nameof(ClassRoomId))]
        [JsonIgnore]
        public ClassRoom? ClassRooms { get; set; }
        public int ClassRoomNotificationId { get; set; }
        [ForeignKey(nameof(ClassRoomNotificationId))]
        [JsonIgnore]
        public ClassRoomNotification? ClassRoomNotifications { get; set; }
    }
}
