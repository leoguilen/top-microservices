using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Interfaces.Infra;

namespace TOP.IdentityService.Infra.Broker.RabbitMq
{
    public class RabbitMqPublisher : IProducer
    {
        private readonly ILogWriter _logWriter;
        private readonly IRabbitMqConnection _connection;
        private readonly RabbitMQConfiguration _rabbitConfig;

        public RabbitMqPublisher(ILogWriter logWriter,
            IRabbitMqConnection connection,
            RabbitMQConfiguration rabbitConfig)
        {
            _logWriter = logWriter;
            _connection = connection;
            _rabbitConfig = rabbitConfig;
        }

        public Task Publish<T>(T message, string routingKey = "")
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                BasicPublish(message, routingKey);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logWriter.Error("Error publishing message in exchange", message, e);
                throw;
            }
        }

        private void BasicPublish(object message, string routingKey)
        {
            using var channel = CreateNewChannel(_rabbitConfig.ExchangeName);

            _logWriter.Info($"Publishing message to RabbitMQ: {JsonSerializer.Serialize(message)} in Exchange: {_rabbitConfig.ExchangeName}");

            var correlationId = _logWriter.CorrelationId;
            var headers = new Dictionary<string, object>
            {
                { "correlation_id", correlationId.ToString() }
            };

            var basicProps = channel.CreateBasicProperties();
            basicProps.Persistent = true;
            basicProps.Headers = headers;

            channel.BasicPublish(
                exchange: _rabbitConfig.ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: basicProps,
                body: SerializeAndEncodeMessage(message));

            channel.WaitForConfirmsOrDie();
        }

        private IModel CreateNewChannel(string exchangeName)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            return _connection.CreateModel(exchangeName);
        }

        private static byte[] SerializeAndEncodeMessage<T>(T message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            return Encoding.UTF8.GetBytes(serializedMessage);
        }
    }
}
