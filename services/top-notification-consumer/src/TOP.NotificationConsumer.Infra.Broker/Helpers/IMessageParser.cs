using RabbitMQ.Client.Events;
using System;

namespace TOP.NotificationConsumer.Infra.Broker.Helpers
{
    public interface IMessageParser
    {
        T ParseMessage<T>(BasicDeliverEventArgs args);
        Guid ParseCorrelationId(BasicDeliverEventArgs args);
    }
}
