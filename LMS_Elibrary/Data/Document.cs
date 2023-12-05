using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace LMS_Elibrary.Data
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Required]
        //[ServiceStack.DataAnnotations.CheckConstraint("ResourceType IN ('bài giảng', 'tài nguyên')")]
        public string Type { get; set; }
        public string? Creator { get; set; }
        public DateTime Date { get; set; }
        public bool? Approved { get; set; }
        public string? Approver { get; set; }
        public string? Note { get; set; }

        public int? LectureID { get; set; }
        [ForeignKey(nameof(LectureID))]
        [JsonIgnore]
        public Lecture? Lecture { get; set; }

        //public ICollection<File>? Files { get; set; }
        public File? File { get; set; }

    }
}
