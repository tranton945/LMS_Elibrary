using LMS_Elibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class Roles : IRoles
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ElibraryDbContext _context;

        public Roles(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ElibraryDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> AddRole(string roleName)
        {
            var role = new IdentityRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);

            return result;
        }

        public async Task<IdentityResult> AddUserRole(string email, string roleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            var role = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<IdentityResult> ChangeUserRole(string email, string newRoleName, string oldRoleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            var newRole = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == newRoleName);
            var oldRole = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == oldRoleName);

            if (user != null && newRole != null)
            {
                var resultRemove = await _userManager.RemoveFromRoleAsync(user, oldRole.Name);

                if (resultRemove.Succeeded)
                {
                    var resultAdd = await _userManager.AddToRoleAsync(user, newRole.Name);
                    if (resultAdd.Succeeded)
                    {
                        return resultAdd;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteRole(string role)
        {
            var _role = _roleManager.Roles.SingleOrDefault(r => r.Name == role);
            if (role != null)
            {
                _roleManager.DeleteAsync(_role);
                return true;
            }
            return false;
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            //var roles = await _roleManager.Roles.ToListAsync();
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetByName(string role)
        {
            var _role = _roleManager.Roles.SingleOrDefault(r => r.Name == role);
            if (_role != null)
            {
                return _role;
            }
            return null;
        }

        public async Task<IdentityResult> RemoveUserRole(string email, string roleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            var role = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (user != null && role != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
