using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;
using TOP.IdentityService.Infra.Data.Context;
using TOP.IdentityService.UnitTest.Common.Extensions;

namespace TOP.IdentityService.UnitTest.ConfigureServices
{
    public class ServicesResolver
    {
        public static IServiceProvider Resolve()
        {
            var serviceProvider = new ServiceCollection();
            var jwtConfig = new JwtConfiguration
            {
                Secret = "9ce891b219b6fb5b0088e3e05e05baf5",
                TokenLifetime = TimeSpan.FromMinutes(5),
                Issuer = "UnitTest",
                Audience = "UnitTest"
            };

            serviceProvider.AddDbContext<AppIdentityDbContext>(options =>
                options.UseInMemoryDatabase(
                    Guid.NewGuid().ToString()),
                    ServiceLifetime.Singleton);

            serviceProvider
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            serviceProvider.AddLogging();
            serviceProvider.AddSingleton(jwtConfig);
            serviceProvider.AddSingleton<IIdentityService, Domain.Services.IdentityService>();

            return serviceProvider
                .BuildServiceProvider()
                .SeedDbTest();
        }
    }
}
