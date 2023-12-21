using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS_Elibrary.Data
{
    public class ClassRoomLectures
    {
        [Key]
        public int Id { get; set; }

        public int ClassRoomID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ClassRoomID))]
        public ClassRoom? ClassRoom { get; set; }
        public int LectureID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(LectureID))]
        public Lecture? Lecture { get; set; }
    }
}
