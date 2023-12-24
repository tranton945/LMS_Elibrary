namespace LMS_Elibrary.Models
{
    public class ExamMCModel
    {
        public string Name { get; set; }
        public string SubjectGroup { get; set; }
        public string Subject { get; set; }
        public int Duration { get; set; }
        public bool isDraft { get; set; }
        public List<CreateMCQuestionAndAnswerModel> QuestionAndAnswerModel { get; set; }

    }
}
