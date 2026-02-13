using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Abstraction;
using System.Linq.Expressions;

namespace Rens_RentCar.Server.Infrastructure;

public static class ExtensionMethods
{
    // Adds QueryFilter isDeleted Query
    public static void ApplyGlobalFilters(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (typeof(BaseEntity).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                entityType.SetQueryFilter(lambda);
            }
        }
    }
}