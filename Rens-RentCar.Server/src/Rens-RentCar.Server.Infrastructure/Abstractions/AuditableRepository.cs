using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Server.Infrastructure.Abstractions;

internal class AuditableRepository<TEntity, TContext> : Repository<TEntity, TContext>, IAuditableRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    private readonly TContext _context;
    public AuditableRepository(TContext context) : base(context) => _context = context;

    public IQueryable<EntityWithAuditDto<TEntity>> GetAllWithAuditInfos()
    {
        var entities = _context.Set<TEntity>().AsNoTracking().AsQueryable();
        var users = _context.Set<User>().AsNoTracking().AsQueryable();

        var result = entities
            .Join(users, m => m.CreatedBy, m => m.Id,
            (b, user) => new { entity = b, createdUser = user })
            .GroupJoin
            (users, m => m.entity.UpdatedBy, m => m.Id,
            (b, user) => new { b.entity, b.createdUser, updatedUser = user })
            .SelectMany
            (s => s.updatedUser.DefaultIfEmpty(),
            (x, updatedUser) => new EntityWithAuditDto<TEntity>
            {
                Entity = x.entity,
                CreatedUser = x.createdUser,
                UpdatedUser = updatedUser
            });
        return result;
    }
}
