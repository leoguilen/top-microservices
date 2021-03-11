using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using TOP.SyncConsumer.Infra.CrossCutting.Ioc.DependencyInjection;

namespace TOP.SyncConsumer.ConsoleApp
{
    public static class Startup
    {
        public static ServiceProvider SetupApplication()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            return new ServiceCollection()
                .AddIoc(configuration)
                .BuildServiceProvider();
        }
    }
}
