using Microsoft.AspNetCore.Identity;

namespace LMS_Elibrary.Services
{
    public interface IRoles
    {
        public Task<IdentityResult> AddRole(string roleName);
        public Task<List<IdentityRole>> GetAllRoles();
        public Task<IdentityRole> GetByName(string role);
        public Task<bool> DeleteRole(string role);
        public Task<IdentityResult> AddUserRole(string email, string roleName);
        public Task<IdentityResult> RemoveUserRole(string email, string roleName);
        public Task<IdentityResult> ChangeUserRole(string email, string newRoleName, string oldRoleName);

    }
}
