using LMS_Elibrary.Services;

namespace LMS_Elibrary.Models
{
    public class DoNotApproveDocument
    {
        public int DocumentId { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Approver { get; set; }
        public string? Note { get; set; }
        public bool SendNotificaion { get; set;}

    }
}
