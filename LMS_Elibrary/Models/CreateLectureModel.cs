namespace LMS_Elibrary.Models
{
    public class CreateLectureModel
    {
        public string Title { get; set; }
        public string? Descriptions { get; set; }
        public int? TopicId { get; set; }
        public bool? BlockStudent { get; set; }
    }
}
