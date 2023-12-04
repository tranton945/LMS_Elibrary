using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SubjectId { get; set; }
        [Required]
        public string SubjectName { get; set; }
        public string? Teacher { get; set; }
        //public bool? Approved { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Descriptions { get; set; }
        public int? ApprovalDocs { get; set; }
        
        
        public ICollection<Topic>? Topics { get; set; }
    }
}
