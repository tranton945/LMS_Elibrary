using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Descriptions { get; set; }
        public bool? BlockStudents { get; set; }


        public int? TopicId { get; set; }
        [ForeignKey(nameof(TopicId))]
        [JsonIgnore]
        public Topic? Topic { get; set; }

        public ICollection<Document>? Documents { get; set; }
        public ICollection<Questions>? Questions { get; set; }

    }
}
