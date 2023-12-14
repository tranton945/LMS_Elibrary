namespace LMS_Elibrary.Models
{
    public class DocumentRoleTeacherDTO
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string? Approver { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
    }
}
