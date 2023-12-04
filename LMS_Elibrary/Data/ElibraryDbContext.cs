using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Data
{
    public class ElibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public ElibraryDbContext(DbContextOptions<ElibraryDbContext> options) : base(options) { }


        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<File> Files { get; set; }
    }
}
