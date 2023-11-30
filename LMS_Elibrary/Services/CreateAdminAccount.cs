using LMS_Elibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace LMS_Elibrary.Services
{
    public class CreateAdminAccount
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateAdminAccount(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }


        public async Task Start()
        {
            await CreateRolesAsync();
            await CreateAdminUserAsync();
        }

        private async Task CreateAdminUserAsync()
        {
            var adminUser = await _userManager.FindByEmailAsync("admin123@admin.com");
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    Name = "admin123",
                    Email = "admin123@admin.com",
                    DateOfBirt = DateTime.Now,
                    Gender = "male",
                    UserName = "admin123@admin.com",
                };

                var createAdminAccount = await _userManager.CreateAsync(admin, "Admin123@admin.com");

                if (createAdminAccount.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }

        }

        private async Task CreateRolesAsync()
        {
            var roleExist = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExist)
            {
                var role = new IdentityRole { Name = "Admin" };
                var result = await _roleManager.CreateAsync(role);
            }
        }
    }
}
