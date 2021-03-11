using System;
using System.Threading.Tasks;
using TOP.SyncConsumer.Domain.Interfaces.Domain.Services;
using TOP.SyncConsumer.Domain.Interfaces.Infra;
using TOP.SyncConsumer.Domain.Models.Message;

namespace TOP.SyncConsumer.Domain.Services
{
    public class SyncService : ISyncService
    {
        private readonly ISyncRepository _syncRepository;
        private readonly ILogWriter _logWriter;

        public SyncService(ISyncRepository syncRepository, ILogWriter logWriter)
        {
            _syncRepository = syncRepository;
            _logWriter = logWriter;
        }

        public async Task SyncUserToProfileDbAsync(UserMessage user, string correlationId)
        {
            try
            {
                await _syncRepository.InsertUserProfile(user);
                await _syncRepository.InsertSyncInfos(correlationId, user);
                _logWriter.Info("informations persisted successfully", user);
            }
            catch (Exception ex)
            {
                _logWriter.Error("error persisting informations", user, ex);
                throw;
            }
        }
    }
}
