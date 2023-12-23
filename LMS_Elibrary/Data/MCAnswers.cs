using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class MCAnswers
    {
        [Key]
        public int id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public ICollection<QuestionAnswerMapping>? QuestionAnswerMapping { get; set; }
    }
}
