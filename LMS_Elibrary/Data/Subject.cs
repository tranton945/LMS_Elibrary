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

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Descriptions { get; set; }
        public string? ApprovalDocs { get; set; }

        //public DateTime? LastTeacherAccessedDate { get; set; }
        public ICollection<SubAccessHistory> SubAccessHistories { get; set; }
        public ICollection<Topic>? Topics { get; set; }
        public ICollection<ClassRoom>? Classes { get; set; }
        public ICollection<SubjectOtherInformation>? SubjectOtherInformations { get; set; }
        public ICollection<Teacher>? Teachers { get; set; }
        public ICollection<Document>? Documents { get; set; }
    }
}
