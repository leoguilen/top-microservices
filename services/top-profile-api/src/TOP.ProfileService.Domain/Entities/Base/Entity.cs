using System;

namespace TOP.ProfileService.Domain.Entities.Base
{
    public abstract class Entity : EntityBase<Guid>
    {
        protected Entity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; set; }
    }
}
