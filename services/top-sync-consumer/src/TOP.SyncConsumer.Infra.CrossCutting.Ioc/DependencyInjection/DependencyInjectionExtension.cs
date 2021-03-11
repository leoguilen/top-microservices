using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TOP.SyncConsumer.Domain.Configurations;
using TOP.SyncConsumer.Domain.Interfaces.Domain.Services;
using TOP.SyncConsumer.Domain.Interfaces.Infra;
using TOP.SyncConsumer.Domain.Services;
using TOP.SyncConsumer.Infra.Broker.Helpers;
using TOP.SyncConsumer.Infra.Broker.RabbitMq;
using TOP.SyncConsumer.Infra.CrossCutting.Logger.Logging;
using TOP.SyncConsumer.Infra.Data.Repositories;

namespace TOP.SyncConsumer.Infra.CrossCutting.Ioc.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddIoc(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddConfigs(configuration)
                .AddInfrastructure()
                .AddDomain();
        }

        private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton(configuration)
                .AddSingleton(new AppConfiguration { ApplicationName = configuration["AppName"] })
                .AddSingleton(new DbConfiguration { ConnectionString = configuration.GetConnectionString("ProfileDbConnection") })
                .AddSingleton(configuration.GetSection("Elastic").Get<ElasticConfiguration>())
                .AddSingleton(configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>());
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<IRabbitMqClient, RabbitMqClient>()
                .AddScoped<IConsumer, RabbitMqConsumer>()
                .AddScoped<IMessageParser, MessageParser>()
                .AddScoped<ILogWriter, LogWriter>()
                .AddScoped<ISyncRepository, SyncRepository>();
        }

        private static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddScoped<ISyncService, SyncService>();
        }
    }
}
