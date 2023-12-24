using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace LMS_Elibrary.Data
{
    public class FavoriteSubjects
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsStar { get; set; }
        public int SubjectID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(SubjectID))]
        public Subject? Subject { get; set; }
    }
}
