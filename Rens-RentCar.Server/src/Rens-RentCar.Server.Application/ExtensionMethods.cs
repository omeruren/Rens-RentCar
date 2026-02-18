using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Server.Application;

internal static class ExtensionMethods
{
    public static IQueryable<EntityWithAuditDto<TEntity>> ApplyAuditDto<TEntity>(this IQueryable<TEntity> entities, IQueryable<User> users)
        where TEntity : BaseEntity
    {
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

