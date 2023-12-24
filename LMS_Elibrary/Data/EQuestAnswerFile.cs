using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class EQuestAnswerFile
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }

        public int? EssayQuestionId { get; set; }
        [ForeignKey(nameof(EssayQuestionId))]
        [JsonIgnore]
        public EssayQuestions? EssayQuestions { get; set; }
    }
}
