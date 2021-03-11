using System;
using System.Threading.Tasks;
using TOP.SyncConsumer.Domain.Models.Message;

namespace TOP.SyncConsumer.Domain.Interfaces.Infra
{
    public interface ISyncRepository : IDisposable
    {
        Task InsertSyncInfos(string correlationId, object data);
        Task InsertUserProfile(UserMessage user);
    }
}
