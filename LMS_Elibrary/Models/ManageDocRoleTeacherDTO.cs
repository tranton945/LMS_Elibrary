namespace LMS_Elibrary.Models
{
    public class ManageDocRoleTeacherDTO
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string? SubjectName { get; set; }
        public string? Updater { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int FileSize { get; set; }

    }
}
