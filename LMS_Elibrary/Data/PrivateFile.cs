using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class PrivateFile
    {
        [Key]
        public int Id { get; set; }
        public string Updator { get; set; }
        public DateTime LastUpdate { get; set; }

        public File? File { get; set; }
    }
}
