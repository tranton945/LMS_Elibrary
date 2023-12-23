using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace LMS_Elibrary.Data
{
    public class QuestionAnswerMapping
    {
        [Key]
        public int id { get; set; }
        public int MCAnswerId { get; set; }
        [ForeignKey(nameof(MCAnswerId))]
        [JsonIgnore]
        public MCAnswers? MCAnswers { get; set; }
        public int MCQuestionId { get; set; }
        [ForeignKey(nameof(MCQuestionId))]
        [JsonIgnore]
        public MCQuestions? MCQuestions { get; set; }
    }
}
