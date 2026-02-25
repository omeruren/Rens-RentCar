using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Categories;

public sealed class CategoryDto : BaseEntityDto
{
    public string Name { get; set; } = default!;
}
public static class CategoryExtensions
{
    public static IQueryable<CategoryDto> MapTo(this IQueryable<EntityWithAuditDto<Category>> entities)
    {
        var res = entities.Select(s => new CategoryDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,

            IsActive = s.Entity.IsActive,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,

            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
            UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null
        })
        .AsQueryable();

        return res;
    }
}