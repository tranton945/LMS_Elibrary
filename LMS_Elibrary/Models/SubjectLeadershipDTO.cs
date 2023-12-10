namespace LMS_Elibrary.Models
{
    public class SubjectLeadershipDTO
    {
        public int ID { get; set; }
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string Teacher { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string ApproveDoc { get; set; }
    }
}
