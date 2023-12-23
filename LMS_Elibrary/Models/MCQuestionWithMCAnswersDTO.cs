using LMS_Elibrary.Data;

namespace LMS_Elibrary.Models
{
    public class MCQuestionWithMCAnswersDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<MCAnswers>? Answers { get; set; }

    }
}
