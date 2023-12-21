namespace LMS_Elibrary.Models
{
    public class DocumentUploadViewDTO
    {
        public int DocumentId { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string Creator { get; set; }
        public DateTime Date { get; set; }
        public int Size { get; set; }

    }
}
