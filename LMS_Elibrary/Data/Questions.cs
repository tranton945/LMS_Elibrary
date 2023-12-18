using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class Questions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contain { get; set; }
        [Required]
        public bool Like { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public bool HiddenAnswer { get; set; }
        [Required]
        public bool Answered { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ClassRoomId { get; set; }
        [ForeignKey(nameof(ClassRoomId))]
        [JsonIgnore]
        public ClassRoom? ClassRoom { get; set; }

        public int? LectureId { get; set; }
        [ForeignKey(nameof(LectureId))]
        [JsonIgnore]
        public Lecture? Lecture { get; set; }

        public int? TopicId { get; set; }
        [ForeignKey(nameof(TopicId))]
        [JsonIgnore]
        public Topic? Topic { get; set; }
        public ICollection<Answers>? Answers { get; set; }
        public ICollection<LikeQuestions>? LikeQuestions { get; set; }
    }
}
