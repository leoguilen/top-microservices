namespace TOP.SyncConsumer.Infra.Data.Sql
{
    public static class SqlStatements
    {
        public const string InsertUserProfileStmt = @"
            INSERT INTO [dbo].[UserProfiles] (Id, UserName, Email, PhoneNumber, CreatedAt, UpdatedAt)
            VALUES (@id, @username, @email, @phoneNumber, GETDATE(), GETDATE())";
        public const string InsertSyncInfos = @"
            INSERT INTO [dbo].[LastSynchronization] (Timestamp, CorrelationId, Data)
            VALUES (GETDATE(), @correlationId, @data)";
    }
}
