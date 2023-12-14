namespace LMS_Elibrary.Models
{
    public class PopupAddDocmentDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<string> Topics { get; set; }
        public List<string>? Lectures { get; set; }
    }
}
