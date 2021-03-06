using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using TOP.NotificationConsumer.Domain.Configurations;
using TOP.NotificationConsumer.Domain.Interfaces.Domain;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;
using TOP.NotificationConsumer.Domain.Services;
using TOP.NotificationConsumer.Infra.Broker.Helpers;
using TOP.NotificationConsumer.Infra.Broker.RabbitMq;
using TOP.NotificationConsumer.Infra.CrossCutting.Logger.Logging;

namespace TOP.NotificationConsumer.Infra.CrossCutting.Ioc.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddIoc(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddConfigs(configuration)
                .AddInfrastructure()
                .AddServices(configuration)
                .AddDomain();
        }

        private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton(configuration)
                .AddSingleton(new AppConfiguration { ApplicationName = configuration["AppName"] })
                .AddSingleton(configuration.GetSection("Elastic").Get<ElasticConfiguration>())
                .AddSingleton(configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>());
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddScoped<IRabbitMqClient, RabbitMqClient>()
                .AddScoped<IConsumer, RabbitMqConsumer>()
                .AddScoped<IMessageParser, MessageParser>()
                .AddScoped<ILogWriter, LogWriter>();
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mailKitOptions = new MailKitOptions()
            {
                Server = configuration["SmtpConfig:Server"],
                Port = Convert.ToInt32(configuration["SmtpConfig:Port"]),
                SenderName = configuration["SmtpConfig:SenderName"],
                SenderEmail = configuration["SmtpConfig:SenderEmail"],
                Account = configuration["SmtpConfig:Account"],
                Password = configuration["SmtpConfig:Password"],
                Security = true
            };

            return services
                .AddMailKit(optBuilder => optBuilder
                    .UseMailKit(mailKitOptions));
        }

        private static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddScoped<INotificationSender, NotificationSender>();
        }
    }
}
