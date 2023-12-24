namespace LMS_Elibrary.Models
{
    public class CreateMCQuestionModel
    {
        public string Level { get; set; }
        public string? Content { get; set; }
        public bool isSingleChoice { get; set; }
        public string SubjectGroup { get; set; }
        public string Subject { get; set; }
        public int? examId { get; set; }
    }
}
