using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.ProtectionPackages.ValueObjects;
using System.Linq;

namespace Rens_RentCar.Domain.ProtectionPackages;

public sealed class ProtectionDto : BaseEntityDto
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public bool IsRecommended { get; set; }
    public List<string> Coverages { get; set; } = new();
}

public static class ProtectionExtensions
{
    public static IQueryable<ProtectionDto> MapTo(this IQueryable<EntityWithAuditDto<ProtectionPackage>> entities)
    {
        var res = entities.Select(s => new ProtectionDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            Price = s.Entity.Price.Value,
            IsRecommended = s.Entity.IsRecommended.Value,
            Coverages = s.Entity.Coverages.Select(c => c.Value).ToList(),

            IsActive = s.Entity.IsActive,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,

            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
            UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null
        }).AsQueryable();

        return res;
    }
}
