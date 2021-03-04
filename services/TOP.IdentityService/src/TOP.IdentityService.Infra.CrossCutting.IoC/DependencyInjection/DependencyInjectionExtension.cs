using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using TOP.IdentityService.Application.Behaviours;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Interfaces.Domain.Holders;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Interfaces.Infra;
using TOP.IdentityService.Domain.Models;
using TOP.IdentityService.Domain.Services.Holders;
using TOP.IdentityService.Infra.Broker.RabbitMq;
using TOP.IdentityService.Infra.CrossCutting.Logger.Logging;
using TOP.IdentityService.Infra.Data.Context;

namespace TOP.IdentityService.Infra.CrossCutting.IoC.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddIoc(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddConfigs(configuration)
                .AddInfrastructure()
                .AddIdentity(configuration)
                .AddServices()
                .AddApplication();
        }

        private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton(configuration)
                .AddSingleton(new AppConfiguration { ApplicationName = configuration["AppName"] })
                .AddSingleton(new DbConfiguration { ConnectionString = configuration.GetConnectionString("IdentityConnection") })
                .AddSingleton(configuration.GetSection("Elastic").Get<ElasticConfiguration>())
                .AddSingleton(configuration.GetSection("Jwt").Get<JwtConfiguration>())
                .AddSingleton(configuration.GetSection("RabbitMq").Get<RabbitMQConfiguration>());
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<IRabbitMqConnection, RabbitMqConnection>()
                .AddScoped<IProducer, RabbitMqPublisher>()
                .AddScoped<ILogWriter, LogWriter>();
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppIdentityDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("IdentityConnection"), opt =>
                {
                    opt.CommandTimeout(180);
                    opt.EnableRetryOnFailure(5);
                    opt.EnableRetryOnFailure(2);
                    opt.MigrationsAssembly("TOP.IdentityService.Infra.Data");
                })
                .EnableSensitiveDataLogging());

            services.AddIdentity<ApplicationUser, IdentityRole>(x =>
            {
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                x.Lockout.MaxFailedAccessAttempts = 5;
                x.Lockout.AllowedForNewUsers = true;

                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = true;
                x.Password.RequireNonAlphanumeric = true;
                x.Password.RequireUppercase = true;
                x.Password.RequiredLength = 6;
                x.Password.RequiredUniqueChars = 1;

                x.SignIn.RequireConfirmedEmail = true;

                x.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                x.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IRequestContextHolder, RequestContextHolder>()
                .AddScoped<IIdentityService, Domain.Services.IdentityService>();
        }

        private static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly
                .GetAssembly(typeof(LoggingRequestBehaviour<,>));

            return services
                .AddMediatR(assembly)
                .AddFluentValidation(new[] { assembly })
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingRequestBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
        }
    }
}
