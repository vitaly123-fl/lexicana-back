using lexicana.TopicFolder.Entities;
using lexicana.TopicFolder.TopicWordFolder.Entities;
using lexicana.TopicFolder.WordFolder.Entities;
using lexicana.UserFolder.Entities;
using lexicana.UserFolder.UserTopicFolder.Entities;
using Microsoft.EntityFrameworkCore;

namespace lexicana.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicWord> TopicWords { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
