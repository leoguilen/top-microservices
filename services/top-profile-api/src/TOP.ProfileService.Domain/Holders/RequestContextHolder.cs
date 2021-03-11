using System;
using TOP.ProfileService.Domain.Interfaces.Domain.Holders;

namespace TOP.ProfileService.Domain.Holders
{
    public class RequestContextHolder : IRequestContextHolder
    {
        public Guid CorrelationId { get; set; }
        public object Object { get; set; }
    }
}
