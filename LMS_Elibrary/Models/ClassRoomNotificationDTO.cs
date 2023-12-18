namespace LMS_Elibrary.Models
{
    public class ClassRoomNotificationDTO
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
    }
}
