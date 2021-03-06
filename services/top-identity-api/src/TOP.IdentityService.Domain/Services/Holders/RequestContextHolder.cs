using System;
using TOP.IdentityService.Domain.Interfaces.Domain.Holders;

namespace TOP.IdentityService.Domain.Services.Holders
{
    public class RequestContextHolder : IRequestContextHolder
    {
        public Guid CorrelationId { get; set; }
        public object Object { get; set; }
    }
}
