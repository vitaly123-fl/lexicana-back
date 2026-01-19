using lexicana.UserFolder.Entities;
using lexicana.LessonFolder.Entites;
using lexicana.TopicFolder.Entities;
using Microsoft.EntityFrameworkCore;
using lexicana.TopicFolder.WordFolder.Entities;
using lexicana.UserFolder.UserLessonFolder.Entities;

namespace lexicana.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<UserLesson> UserLessons { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
