using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using TOP.IdentityService.Domain.Constants;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.UnitTest.Common.Extensions
{
    public static class ServicesResolverExtensions
    {
        public static IServiceProvider SeedDbTest(this IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedFakeData(userManager, roleManager);

            return provider;
        }

        private static void SeedFakeData(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            userManager.CreateAsync(new ApplicationUser { Email = "test1@email.com", EmailConfirmed = true, UserName = "test1" }, "Test123#");
            userManager.CreateAsync(new ApplicationUser { Email = "test2@email.com", EmailConfirmed = false, UserName = "test2" }, "Test123#");
            userManager.CreateAsync(new ApplicationUser { Email = "test3@email.com", EmailConfirmed = true, UserName = "test3", PhoneNumber = "(11)1234-5678", PhoneNumberConfirmed = true }, "Test123#");

            roleManager.CreateAsync(new IdentityRole(Roles.ADMINISTRADOR));
            roleManager.CreateAsync(new IdentityRole(Roles.MANAGER));
            roleManager.CreateAsync(new IdentityRole(Roles.DEFAULT));

            var test1User = userManager.FindByNameAsync("test1").Result;
            var test2User = userManager.FindByNameAsync("test2").Result;
            var test3User = userManager.FindByNameAsync("test3").Result;

            userManager.AddToRoleAsync(test1User, Roles.DEFAULT);
            userManager.AddToRoleAsync(test2User, Roles.DEFAULT);
            userManager.AddToRoleAsync(test3User, Roles.DEFAULT);
        }
    }
}
