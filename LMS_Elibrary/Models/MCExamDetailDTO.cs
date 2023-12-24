namespace LMS_Elibrary.Models
{
    public class MCExamDetailDTO
    {
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public List<QuestionWithAnswers> QuestionWithAnswers { get; set; }
        public List<string> CorrectAnswer { get; set; }
    }
}
