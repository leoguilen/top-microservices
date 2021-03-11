using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TOP.ProfileService.Domain.Entities;

namespace TOP.ProfileService.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileDetails> UserProfileDetails { get; set; }
        private DbSet<LastSynchronization> LastSynchronizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
