using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        [JsonIgnore]
        public Subject? Subject { get; set; }

        public ICollection<ClassRoom> Classrooms { get; set; }
    }
}
