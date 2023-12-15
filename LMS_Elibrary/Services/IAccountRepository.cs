using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Identity;

namespace LMS_Elibrary.Services
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);

        public Task<string> SignInAsync(SignInModel model);

        public Task<bool> SignOut();

        public Task<List<ApplicationUser>> GetAllAccounts();
        public Task<ApplicationUser> GetAccountsByEmail(string email);
        public Task<bool> UpdateAccount(string email, string newName, DateTime newDateOfBirt, string newGender, string newPhoneNumber, string newTeacherID, string newAddress);
        public Task<bool> UpdatePassword(ChangePasswordModel model);
        public Task<ResultUpdateAvatar> UpdateAvatar(IFormFile avatarFile);
        public Task<bool> DeleteAccount(string email);

        public Task<AccountWithRolesDto> GetAccountRole(string email);
        public Task<List<AccountWithRolesDto>> GetAllAccountRole();
    }
}
