using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class Answers
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }        
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public int QuenstionId { get; set; }
        [ForeignKey(nameof(QuenstionId))]
        [JsonIgnore]
        public Questions? Questions { get; set; }
    }
}
