using System.Threading.Tasks;
using TOP.SyncConsumer.Domain.Models.Message;

namespace TOP.SyncConsumer.Domain.Interfaces.Domain.Services
{
    public interface ISyncService
    {
        Task SyncUserToProfileDbAsync(UserMessage user, string correlationId);
    }
}
