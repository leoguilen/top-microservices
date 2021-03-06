using RabbitMQ.Client;
using System;
using TOP.NotificationConsumer.Domain.Configurations;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;

namespace TOP.NotificationConsumer.Infra.Broker.RabbitMq
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogWriter _logWriter;

        private IConnection _connection;
        private bool _disposed;

        public RabbitMqClient(ILogWriter logWriter, RabbitMqConfiguration config)
        {
            _logWriter = logWriter;
            _connectionFactory = BuildConnectionFactory(config);

            Connect();
            DeclareQueue(config.QueueName);
        }

        public IModel Channel { get; private set; }

        private static ConnectionFactory BuildConnectionFactory(RabbitMqConfiguration rabbitMqConfig) => 
            new ConnectionFactory
            {
                HostName = rabbitMqConfig.HostName,
                UserName = rabbitMqConfig.UserName,
                Password = rabbitMqConfig.Password,
                VirtualHost = rabbitMqConfig.VirtualHost,
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true
            };

        private void Connect()
        {
            try
            {
                _connection?.Dispose();
                _connection = _connectionFactory.CreateConnection();
                Channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                _logWriter.Fatal("error establishing connection", ex);
            }
        }

        private void DeclareQueue(string QueueName)
        {
            Channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
        }

        #region Dispose Pattern

        ~RabbitMqClient() => Dispose(false);

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
