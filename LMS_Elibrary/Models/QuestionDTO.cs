using LMS_Elibrary.Data;

namespace LMS_Elibrary.Models
{
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        public string userAvatar { get; set; }
        public string userName { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? NumberOfAnswer { get; set; }
        public int? NumberOfLike { get; set; }
        public ICollection<Answers>? Answers { get; set; }

    }
}
