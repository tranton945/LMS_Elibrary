using Microsoft.AspNetCore.Identity;

namespace LMS_Elibrary.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? DateOfBirt { get; set; }
        public string Gender { get; set; } = null!;
        public string? Avatar { get; set; } = "/StaticFiles/images/avatars/default_avatar.jpg";
        public string? TeacherID { get; set; }
        //public int? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }
}
