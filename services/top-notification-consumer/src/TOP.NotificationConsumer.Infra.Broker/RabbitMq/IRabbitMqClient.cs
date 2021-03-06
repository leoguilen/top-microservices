using RabbitMQ.Client;
using System;

namespace TOP.NotificationConsumer.Infra.Broker.RabbitMq
{
    public interface IRabbitMqClient : IDisposable
    {
        public IModel Channel { get; }
    }
}
