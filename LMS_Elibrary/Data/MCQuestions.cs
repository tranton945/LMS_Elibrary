using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class MCQuestions
    {
        [Key]
        public int Id { get; set; }
        public string MCKey { get; set; }
        public string Level { get; set; }
        public string? Content { get; set; }
        public bool isSingleChoice { get; set; }
        public string? Creater { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Updator { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        [JsonIgnore]
        public Subject? Subject { get; set; }
        public int SubjectGroupId { get; set; }
        [ForeignKey(nameof(SubjectGroupId))]
        [JsonIgnore]
        public SubjectGroup? SubjectGroup { get; set; }
        public int? ExamId { get; set; }
        [ForeignKey(nameof(ExamId))]
        [JsonIgnore]
        public Exams? Exams { get; set; }

        public ICollection<QuestionAnswerMapping>? QuestionAnswerMapping { get; set; }
        public ICollection<MCQuestionFiles>? MCQuestionFile { get; set; }
    }
}
