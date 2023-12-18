namespace LMS_Elibrary.Models
{
    public class CreateClassRoomNotificationModel
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public DateTime Date { get; set; }
        public List<string>? StudentID { get; set; }
        public List<string> ClassRoom { get; set; }
    }
}
