using RabbitMQ.Client;
using System;

namespace TOP.IdentityService.Infra.Broker.RabbitMq
{
    public interface IRabbitMqConnection : IDisposable
    {
        bool IsConnected { get; }
        IModel CreateModel(string exchangeName);
        void TryConnect();
    }
}
