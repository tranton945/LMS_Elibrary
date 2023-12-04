using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string Updator { get; set; }
        public DateTime LastUpdate { get; set; }

        public int? DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        [JsonIgnore]
        public Document? Document { get; set; }

    }
}
