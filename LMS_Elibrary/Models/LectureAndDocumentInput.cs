namespace LMS_Elibrary.Models
{
    public class LectureAndDocumentInput
    {
        public CreateLectureModel Lecture { get; set; }
        public List<IFormFile> Lessons { get; set; }
        public List<IFormFile> Resources { get; set; }
        public List<int> DocIds { get; set; }
        public bool AssignDocument { get; set; }
        public List<string>? ClassRooms { get; set; }
    }
}
