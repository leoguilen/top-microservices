namespace TOP.ProfileService.Domain.Entities.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
