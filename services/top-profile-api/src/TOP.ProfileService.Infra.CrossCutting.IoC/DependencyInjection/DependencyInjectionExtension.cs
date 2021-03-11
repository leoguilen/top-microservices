using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TOP.ProfileService.Application.Behaviours;
using TOP.ProfileService.Domain.Configurations;
using TOP.ProfileService.Domain.Holders;
using TOP.ProfileService.Domain.Interfaces.Domain.Holders;
using TOP.ProfileService.Domain.Interfaces.Infra;
using TOP.ProfileService.Infra.CrossCutting.Logger.Logging;
using TOP.ProfileService.Infra.Data.Context;
using TOP.ProfileService.Infra.Data.Repositories;

namespace TOP.ProfileService.Infra.CrossCutting.IoC.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddIoc(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddConfigs(configuration)
                .AddInfrastructure(configuration)
                .AddServices()
                .AddApplication();
        }

        private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton(configuration)
                .AddSingleton(new AppConfiguration { ApplicationName = configuration["AppName"] })
                .AddSingleton(new DbConfiguration { ConnectionString = configuration.GetConnectionString("ProfileDbConnection") })
                .AddSingleton(configuration.GetSection("Elastic").Get<ElasticConfiguration>());
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseSqlServer(configuration.GetConnectionString("ProfileDbConnection"), opt =>
                    {
                        opt.CommandTimeout(180);
                        opt.EnableRetryOnFailure(5);
                        opt.EnableRetryOnFailure(2);
                        opt.MigrationsAssembly("TOP.ProfileService.Infra.Data");
                    })
                    .EnableSensitiveDataLogging();
                })
                .AddScoped<IUserProfileRepository, UserProfileRepository>()
                .AddScoped<ILogWriter, LogWriter>();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IRequestContextHolder, RequestContextHolder>();
        }

        private static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly
                .GetAssembly(typeof(LoggingRequestBehaviour<,>));

            return services
                .AddAutoMapper(assembly)
                .AddMediatR(assembly)
                .AddFluentValidation(new[] { assembly })
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingRequestBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
        }
    }
}
