using System;

namespace TOP.IdentityService.Domain.Interfaces.Domain.Holders
{
    public interface IRequestContextHolder
    {
        public Guid CorrelationId { get; set; }
        public object Object { get; set; }
    }
}
