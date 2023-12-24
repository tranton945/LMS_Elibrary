namespace LMS_Elibrary.Models
{
    public class CreateExamFromBank
    {
        public string ExamName { get; set; }
        public string SubjectGroup { get; set; }
        public string Subject { get; set; }
        public int NumberOfExam { get; set; }
        public int Mark { get; set; }
        public int NumberOfQuestion { get; set; }
        public int NumberOfLowDifficultyQuestion { get; set; }
        public int NumberOfMediumDifficultyQuestion { get; set; }
        public int NumberOfHighDifficultyQuestion { get; set; }
    }
}
