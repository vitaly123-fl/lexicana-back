using lexicana.UserFolder.Entities;
using Microsoft.EntityFrameworkCore;

namespace lexicana.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
