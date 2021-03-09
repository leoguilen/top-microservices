using System;

namespace TOP.ProfileService.Domain.Interfaces.Domain.Holders
{
    public interface IRequestContextHolder
    {
        public Guid CorrelationId { get; set; }
        public object Object { get; set; }
    }
}
