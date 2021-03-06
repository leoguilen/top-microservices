using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Infra.Data.Context
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions options) : base(options) { }
        protected AppIdentityDbContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole(Domain.Constants.Roles.ADMINISTRADOR),
                new IdentityRole(Domain.Constants.Roles.MANAGER),
                new IdentityRole(Domain.Constants.Roles.DEFAULT));
            base.OnModelCreating(builder);  
        }
    }
}
