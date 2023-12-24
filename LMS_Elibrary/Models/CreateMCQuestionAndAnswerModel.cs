using LMS_Elibrary.Data;

namespace LMS_Elibrary.Models
{
    public class CreateMCQuestionAndAnswerModel
    {
        public string? Content { get; set; }
        public string SubjectGroup { get; set; }
        public string Subject { get; set; }
        public List<MCAnswers> MCAnswers { get; set; }
    }
}
