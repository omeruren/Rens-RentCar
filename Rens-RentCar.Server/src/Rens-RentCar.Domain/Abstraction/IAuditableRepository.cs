using GenericRepository;
using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Domain.Abstraction;

public interface IAuditableRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<EntityWithAuditDto<TEntity>> GetAllWithAuditInfos();
}
public sealed class EntityWithAuditDto<TEntity> where TEntity : BaseEntity
{
    public TEntity Entity { get; set; } = default!;
    public User CreatedUser { get; set; } = default!;
    public User? UpdatedUser { get; set; }
}
