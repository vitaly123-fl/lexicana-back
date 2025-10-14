using lexicana.UserFolder.Entities;
using Microsoft.EntityFrameworkCore;
using lexicana.UserFolder.ProviderFolder.Entities;

namespace lexicana.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
