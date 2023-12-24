namespace LMS_Elibrary.Models
{
    public class ExamDTO
    {
        public int ExamId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
    }
}
