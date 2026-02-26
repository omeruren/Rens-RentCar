using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Extras;

public sealed class ExtraDto : BaseEntityDto
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
}

public static class ExtraExtensions
{
    public static IQueryable<ExtraDto> MapTo(this IQueryable<EntityWithAuditDto<Extra>> entities)
    {
        return entities.Select(s => new ExtraDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            Price = s.Entity.Price.Value,
            Description = s.Entity.Description.Value,

            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,

            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
            UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,

            IsActive = s.Entity.IsActive,
        });
    }
}