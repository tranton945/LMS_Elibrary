using Microsoft.AspNetCore.Identity;

namespace LMS_Elibrary.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirt { get; set; }
        public string Gender { get; set; } = null!;
    }
}
