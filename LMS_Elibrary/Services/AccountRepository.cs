using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Drawing;
using System.Linq;

namespace LMS_Elibrary.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ElibraryDbContext _context;
        private readonly BlacklistService _blacklistService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GetUser _getUser;

        public AccountRepository(GetUser getUser, IWebHostEnvironment webHostEnvironment, BlacklistService blacklistService, ElibraryDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _blacklistService = blacklistService;
            _webHostEnvironment = webHostEnvironment;
            _getUser = getUser;
        }
        public async Task<bool> DeleteAccount(string email)
        {
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (accounts == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(accounts);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ApplicationUser> GetAccountsByEmail(string email)
        {
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (accounts == null)
            {
                return null;
            }
            return accounts;
        }

        public async Task<List<ApplicationUser>> GetAllAccounts()
        {
            var accounts = await _userManager.Users.ToListAsync();
            return accounts;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            // login fail
            if (!result.Succeeded)
            {
                return string.Empty;
            }

            var autClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var listUserRoles = await GetRolesOfUser(model.Email);
            // Thêm tất cả các claim về role vào danh sách claim
            autClaims.AddRange(listUserRoles);

            var authenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: autClaims,
                signingCredentials: new SigningCredentials(authenkey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<List<Claim>> GetRolesOfUser(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            // lây danh sách role của user
            var roles = await _userManager.GetRolesAsync(user);
            // Tạo các claim cho từng role
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            return roleClaims;
        }

        public async Task<bool> SignOut()
        {
            var authResult = _httpContextAccessor.HttpContext.AuthenticateAsync().Result;
            var token = authResult.Properties.GetTokenValue("access_token");
            if (await _blacklistService.CheckJWT() == false)
            {
                var tokenBlacklist = new BlacklistedToken
                {
                    Token = token,
                };
                await _context.BlacklistedTokens.AddAsync(tokenBlacklist);

            }
            await _context.SaveChangesAsync();
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                DateOfBirt = model.DateOfBirt,
                Gender = model.Gender,
                UserName = model.Email,
            };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<bool> UpdateAccount(string email, string newName, DateTime newDateOfBirt, string newGender, string newPhoneNumber, string newTeacherID, string newAddress)
        {
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (accounts == null)
            {
                return false;
            }
            var oldName = accounts.Name;
            var oldDateOfBirt = accounts.DateOfBirt;
            var oldGender = accounts.Gender;
            var oldPhoneNumber = accounts.PhoneNumber;
            var oldTeacherID = accounts.TeacherID;
            var oldAddress = accounts.Address;


            accounts.Name = newName ?? accounts.Name;
            accounts.DateOfBirt = newDateOfBirt;
            accounts.Gender = newGender ?? accounts.Gender;
            accounts.PhoneNumber = newPhoneNumber ?? accounts.PhoneNumber;
            accounts.TeacherID = newTeacherID ?? accounts.TeacherID;
            accounts.Address = newAddress ?? accounts.Address;

            var result = await _userManager.UpdateAsync(accounts);

            if (result.Succeeded)
            {
                // Xóa bộ nhớ đệm để tránh trường hợp data cũ vẫn được sử dụng
                await _userManager.UpdateSecurityStampAsync(accounts);

                return true;
            }
            else
            {
                // Khôi phục lại data cũ nếu cập nhật không thành công
                accounts.Name = oldName;
                accounts.DateOfBirt = oldDateOfBirt;
                accounts.Gender = oldGender;

                return false;
            }
        }

        public async Task<bool> UpdatePassword(ChangePasswordModel model)
        {
            var account = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (account == null)
            {
                return false;
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                return false;
            }
            // tạo một token để đặt lại mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(account);
            var result = await _userManager.ResetPasswordAsync(account, token, model.NewPassword);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<AccountWithRolesDto> GetAccountRole(string email)
        {
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (accounts == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(accounts);

            var accountWithRoles = new AccountWithRolesDto
            {
                UserName = accounts.UserName,
                Email = accounts.Email,
                Name = accounts.Name,
                Roles = roles.ToList()
            };

            return accountWithRoles;
        }

        public async Task<List<AccountWithRolesDto>> GetAllAccountRole()
        {
            var accounts = await _userManager.Users.ToArrayAsync();
            if (accounts.Count() == 0)
            {
                return new List<AccountWithRolesDto>();
            }
            var result = new List<AccountWithRolesDto>();
            foreach( var account in accounts)
            {
                var roles = await _userManager.GetRolesAsync(account);

                var accountWithRoles = new AccountWithRolesDto
                {
                    UserName = account.UserName,
                    Email = account.Email,
                    Name = account.Name,
                    Roles = roles.ToList()
                };
                result.Add(accountWithRoles);
            }

            return result;

        }
        private static bool IsImageFile(IFormFile file)
        {
            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return allowedExtensions.Contains(extension);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<ResultUpdateAvatar> UpdateAvatar(IFormFile avatarFile)
        {
            var isusser = await _getUser.user();
            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == isusser.UserName);

            if (accounts == null)
            {
                return new ResultUpdateAvatar
                {
                    text = "Account not found"
                };
            }

            if (avatarFile != null && avatarFile.Length > 0)
            {
                // Kiểm tra loại tệp tin
                if (!IsImageFile(avatarFile))
                {
                    return new ResultUpdateAvatar
                    {
                        text = "Invalite Image"
                    };
                }

                // Xử lý tải lên và lưu trữ ảnh mới
                //_webHostEnvironment.ContentRootPath: Trả về đường dẫn đến thư mục gốc của dự án
                var uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "StaticFiles/images/avatars");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarFile.FileName;
                var newFilePath = Path.Combine(uploadPath, uniqueFileName);
                // tạo file tại đường dẫn newFilePath
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }
                var oldAvatarFileName = accounts.Avatar;
                // Cập nhật đường dẫn mới trong cơ sở dữ liệu
                accounts.Avatar = "/StaticFiles/images/avatars/" + uniqueFileName;
                await _userManager.UpdateAsync(accounts);

                if (!string.IsNullOrEmpty(oldAvatarFileName) && !oldAvatarFileName.EndsWith("default_avatar.jpg"))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, oldAvatarFileName.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                return new ResultUpdateAvatar
                {
                    text = "Update Avatar Success",
                };
            }
            else
            {
                return new ResultUpdateAvatar
                {
                    text = "",
                };
            }
        }
    }
}
