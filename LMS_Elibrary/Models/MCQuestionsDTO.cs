namespace LMS_Elibrary.Models
{
    public class MCQuestionsDTO
    {
        public int MCQuestionId { get; set; }
        public string MCQuesKey { get; set; }
        public string Level { get; set; }
        public string Content { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Updator { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
