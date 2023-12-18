using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class LikeQuestions
    {
        [Key] 
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        [JsonIgnore]
        public Questions? Questions { get; set; }

        
    }
}
