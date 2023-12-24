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
        public DbSet<PrivateFile> PrivateFiles { get; set; }
        public DbSet<SubAccessHistory> SubAccessHistories { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<LikeQuestions> LikeQuestions { get; set; }
        public DbSet<ClassRoomNotification> ClassRoomNotifications { get; set; }
        public DbSet<ClassRoomNotificationLink> ClassRoomNotificationLinks { get; set; }
        public DbSet<SelectedUser> SelectedUsers { get; set; }
        public DbSet<SubjectOtherInformation> SubjectOtherInformations { get; set; }
        public DbSet<ClassRoomLectures> ClassRoomLectures { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SubjectGroup> SubjectGroups { get; set; }
        public DbSet<MCQuestions> MCQuestions { get; set; }
        public DbSet<MCAnswers> MCAnswers { get; set; }
        public DbSet<QuestionAnswerMapping> QuestionAnswerMapping { get; set; }
        public DbSet<MCQuestionFiles> MCQuestionFiles { get; set; }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<EssayQuestions> EssayQuestions { get; set; }
        public DbSet<EQuestAnswerFile> EQuestAnswerFiles { get; set; }
    }
}
