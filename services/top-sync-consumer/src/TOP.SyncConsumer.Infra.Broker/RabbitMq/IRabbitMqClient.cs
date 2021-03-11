using RabbitMQ.Client;
using System;

namespace TOP.SyncConsumer.Infra.Broker.RabbitMq
{
    public interface IRabbitMqClient : IDisposable
    {
        public IModel Channel { get; }
    }
}
