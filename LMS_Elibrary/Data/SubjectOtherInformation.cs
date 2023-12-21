using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class SubjectOtherInformation
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        [JsonIgnore]
        public Subject? Subject { get; set; }

    }
}
