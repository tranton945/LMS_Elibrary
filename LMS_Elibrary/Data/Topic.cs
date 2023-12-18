using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TopicName { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        [JsonIgnore]
        public Subject? Subject { get; set; }

        public ICollection<Lecture>? Lecture { get; set; }
        public ICollection<Questions>? Questions { get; set; }
    }
}
