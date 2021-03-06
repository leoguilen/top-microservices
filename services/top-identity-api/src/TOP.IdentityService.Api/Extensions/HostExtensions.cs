using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TOP.IdentityService.Infra.Data.Context;

namespace TOP.IdentityService.WebApi.Extensions
{
    public static class HostExtensions
    {
        public async static Task<IHost> MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider
                .GetRequiredService<AppIdentityDbContext>();

            await appContext.Database.MigrateAsync();
            return host;
        }
    }
}
