using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;

namespace TOP.NotificationConsumer.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main()
        {
            await using var app = Startup.SetupApplication();
            await app.GetRequiredService<IConsumer>().Consume();
            await Task.Delay(Timeout.Infinite);
        }
    }
}
