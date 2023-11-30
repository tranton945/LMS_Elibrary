using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Data
{
    public class ElibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public ElibraryDbContext(DbContextOptions<ElibraryDbContext> options) : base(options) { }


        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
    }
}
