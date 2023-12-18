using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class ClassRoom
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ClassRoomId { get; set; }
        [Required]
        public string ClassRoomName { get; set; }

        public int SubjectId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }

        public ICollection<Questions>? Questions { get; set; }
    }
}
