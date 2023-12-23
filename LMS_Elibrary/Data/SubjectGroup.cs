using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class SubjectGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<MCQuestions>? MCQuestions { get; set;}
    }
}
