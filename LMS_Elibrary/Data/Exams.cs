using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class Exams
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string ExamName { get; set; }
        public string Format { get; set; }
        public int Duration { get; set; }
        public string? Creator { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public bool Approved { get; set; }
        public bool isDraft { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        [JsonIgnore]
        public Subject? Subject { get; set; }
        public int SubjectGroupId { get; set; }
        [ForeignKey(nameof(SubjectGroupId))]
        [JsonIgnore]
        public SubjectGroup? SubjectGroup { get; set; }

        public ICollection<MCQuestions>? MCQuestions { get; set; }
        public ICollection<EssayQuestions>? EssayQuestions { get; set; }
    }
}
