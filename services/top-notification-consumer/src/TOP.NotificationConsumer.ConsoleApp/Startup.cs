using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TOP.NotificationConsumer.Infra.CrossCutting.Ioc.DependencyInjection;

namespace TOP.NotificationConsumer.ConsoleApp
{
    public static class Startup
    {
        public static ServiceProvider SetupApplication()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            return new ServiceCollection()
                .AddIoc(configuration)
                .BuildServiceProvider();
        }
    }
}
