using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class EssayQuestions
    {
        [Key]
        public int Id { get; set; }
        public string? Answer { get; set; }
        public bool UploadFile { get; set; }
        public int ExamId { get; set; }
        [ForeignKey(nameof(ExamId))]
        [JsonIgnore]
        public Exams? Exams { get; set; }
        public ICollection<EQuestAnswerFile>? EQuestAnswerFiles { get; set; }
    }
}
