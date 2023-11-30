using System.ComponentModel.DataAnnotations;

namespace LMS_Elibrary.Data
{
    public class BlacklistedToken
    {
        [Key]
        public int Id { get; set; }
        public string? Token { get; set; }
    }
}
