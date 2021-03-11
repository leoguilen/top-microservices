using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TOP.SyncConsumer.Domain.Interfaces.Infra;

namespace TOP.SyncConsumer.ConsoleApp
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
