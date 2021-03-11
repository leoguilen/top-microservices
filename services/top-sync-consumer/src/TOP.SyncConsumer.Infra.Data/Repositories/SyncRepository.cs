using Dapper;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using TOP.SyncConsumer.Domain.Configurations;
using TOP.SyncConsumer.Domain.Interfaces.Infra;
using TOP.SyncConsumer.Domain.Models.Message;
using TOP.SyncConsumer.Infra.Data.Sql;

namespace TOP.SyncConsumer.Infra.Data.Repositories
{
    public class SyncRepository : ISyncRepository
    {
        private readonly DbConnection _connection;

        public SyncRepository(DbConfiguration dbConfig)
        {
            _connection = new SqlConnection(dbConfig.ConnectionString);
            _connection.Open();
        }

        public async Task InsertSyncInfos(string correlationId, object data)
        {
            var jsonData = JsonSerializer.Serialize(data);

            await _connection.ExecuteAsync(SqlStatements.InsertSyncInfos, 
                new { correlationId, data = jsonData });
        }

        public async Task InsertUserProfile(UserMessage user)
        {
            await _connection.ExecuteAsync(SqlStatements.InsertUserProfileStmt,
                new
                {
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber
                });
        }

        #region Dispose Pattern

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (_disposed)
            {
                return;
            }

            if (dispose)
            {
                _connection?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
