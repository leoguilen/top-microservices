using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TOP.ProfileService.Infra.Data.Context;

namespace TOP.ProfileService.Infra.Data
{
    public static class MigrationManager
    {
        public async static Task<IHost> MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            await appContext.Database.MigrateAsync();

            return host;
        }
    }
}
