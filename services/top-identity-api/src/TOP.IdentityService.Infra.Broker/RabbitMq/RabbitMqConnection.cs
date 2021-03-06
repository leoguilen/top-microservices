using RabbitMQ.Client;
using System;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Interfaces.Infra;

namespace TOP.IdentityService.Infra.Broker.RabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly RabbitMQConfiguration _rabbitConfig;
        private readonly ILogWriter _logWriter;

        private IConnection _connection;
        private bool _disposed;

        public RabbitMqConnection(ILogWriter logWriter, RabbitMQConfiguration rabbitConfig)
        {
            _logWriter = logWriter;
            _rabbitConfig = rabbitConfig;
            _connectionFactory = SetupConnection(rabbitConfig);
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel(string exchangeName)
        {
            if (IsConnected is false)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: _rabbitConfig.NotificationQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueDeclare(
                queue: _rabbitConfig.SincronizationQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.ExchangeDeclare(
                exchangeName, 
                type: "direct", 
                durable: true, 
                autoDelete: false, 
                arguments: null);

            channel.QueueBind(queue: _rabbitConfig.NotificationQueue, exchange: exchangeName, routingKey: "register_user");
            channel.QueueBind(queue: _rabbitConfig.SincronizationQueue, exchange: exchangeName, routingKey: "register_user");
            channel.QueueBind(queue: _rabbitConfig.NotificationQueue, exchange: exchangeName, routingKey: "reset_user_pwd");

            channel.ConfirmSelect();

            return channel;
        }

        public void TryConnect()
        {
            _connection?.Dispose();
            _connection = _connectionFactory.CreateConnection();

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                return;
            }

            _logWriter.Fatal("RabbitMQ connections could not be created and opened");
        }

        private static IConnectionFactory SetupConnection(RabbitMQConfiguration rabbitConfig)
        {
            return new ConnectionFactory
            {
                HostName = rabbitConfig.HostName,
                Port = rabbitConfig.Port,
                UserName = rabbitConfig.UserName,
                Password = rabbitConfig.Password,
                VirtualHost = rabbitConfig.VirtualHost
            };
        }

        private void OnConnectionBlocked(object sender, RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            TryConnect();
        }

        private void OnCallbackException(object sender, RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            TryConnect();
        }

        ~RabbitMqConnection() => Dispose(true);

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
    }
}
