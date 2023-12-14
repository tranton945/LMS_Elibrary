using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class SubAccessHistory
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime AccessDate { get; set; }

        public int SubjectId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }

    }
}
