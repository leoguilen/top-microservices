using System;

namespace TOP.ProfileService.Domain.Entities
{
    public class LastSynchronization
    {
        public DateTime Timestamp { get; set; }
        public Guid CorrelationId { get; set; }
        public string Data { get; set; }
    }
}
