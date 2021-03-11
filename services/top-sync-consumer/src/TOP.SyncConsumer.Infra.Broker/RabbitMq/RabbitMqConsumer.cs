using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using TOP.SyncConsumer.Domain.Configurations;
using TOP.SyncConsumer.Domain.Interfaces.Domain.Services;
using TOP.SyncConsumer.Domain.Interfaces.Infra;
using TOP.SyncConsumer.Domain.Models.Message;
using TOP.SyncConsumer.Infra.Broker.Helpers;

namespace TOP.SyncConsumer.Infra.Broker.RabbitMq
{
    public class RabbitMqConsumer : IConsumer
    {
        private readonly RabbitMqConfiguration _config;
        private readonly IRabbitMqClient _client;
        private readonly IMessageParser _parser;
        private readonly ILogWriter _logWriter;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqConsumer(
            RabbitMqConfiguration config,
            IRabbitMqClient client,
            IMessageParser parser,
            ILogWriter logWriter,
            IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _client = client;
            _parser = parser;
            _logWriter = logWriter;
            _scopeFactory = scopeFactory;
        }

        public Task Consume()
        {
            var correlationId = Guid.NewGuid();
            _logWriter.Info("starting consumer");

            var consumer = new AsyncEventingBasicConsumer(_client.Channel);
            consumer.Received += async (_, e) => await OnReceive(e);

            _client.Channel.BasicConsume(_config.QueueName, false, consumer);
            _logWriter.Info("consumer started");

            return Task.CompletedTask;
        }

        private async Task OnReceive(BasicDeliverEventArgs args)
        {
            var correlationId = _parser.ParseCorrelationId(args);

            try
            {
                await HandleMessage(args, correlationId);
            }
            catch (Exception ex)
            {
                HandleConsumeError(args, correlationId, ex);
            }
        }

        private async Task HandleMessage(BasicDeliverEventArgs args, Guid correlationId)
        {
            using var scope = _scopeFactory.CreateScope();
            
            var message = GetValidMessage(args);
                var service = scope.ServiceProvider.GetRequiredService<ISyncService>();

            await service.SyncUserToProfileDbAsync(message, correlationId.ToString());

            _client.Channel.BasicAck(args.DeliveryTag, false);
            _logWriter.CorrelationId = correlationId;
            _logWriter.Info("message consumed successfully", message);
        }

        private UserMessage GetValidMessage(BasicDeliverEventArgs args)
        {
            var message = _parser.ParseMessage<UserMessage>(args);
            return message;
        }

        private void HandleConsumeError(BasicDeliverEventArgs args, Guid correlationId, Exception ex)
        {
            var body = args.Body.ToArray();
            var payload = Encoding.UTF8.GetString(body);

            _logWriter.Error("error consuming message", payload, ex);
            _logWriter.CorrelationId = correlationId;
            _client.Channel.BasicNack(args.DeliveryTag, false, true);
        }
    }
}
