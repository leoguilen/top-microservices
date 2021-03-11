using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TOP.SyncConsumer.Domain.Constants;

namespace TOP.SyncConsumer.Infra.Broker.Helpers
{
    public class MessageParser : IMessageParser
    {
        public Guid ParseCorrelationId(BasicDeliverEventArgs args)
        {
            var headers = args.BasicProperties?.Headers;

            return headers is null
                ? Guid.NewGuid()
                : ExtractCorrelationId(headers);
        }

        public T ParseMessage<T>(BasicDeliverEventArgs args)
        {
            var payload = Encoding.UTF8.GetString(args.Body.ToArray());
            return JsonSerializer.Deserialize<T>(payload);
        }

        private static Guid ExtractCorrelationId(IDictionary<string, object> headers)
        {
            return headers.TryGetValue(Headers.BrokerCorrelationId, out var headerId)
                ? ExtractCorrelationId(headerId.ToString())
                : Guid.NewGuid();
        }

        private static Guid ExtractCorrelationId(string headerId)
        {
            return Guid.TryParse(headerId, out var guid)
                ? guid
                : Guid.NewGuid();
        }
    }
}
