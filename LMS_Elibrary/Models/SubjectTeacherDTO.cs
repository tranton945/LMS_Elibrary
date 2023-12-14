using LMS_Elibrary.Data;

namespace LMS_Elibrary.Models
{
    public class SubjectTeacherDTO
    {
        public int Id { get; set; }
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string Descriptions { get; set; }
        public string Status { get; set; }
        public string ApproveDoc { get; set; }
        public ICollection<ClassRoom>? classRooms { get; set; }
    }
}
