using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Roles;

public sealed class RoleDto : BaseEntityDto
{
    public string Name { get; set; } = default!;
    public string PermissionCount { get; set; } = default!;
    public List<string> Permissions { get; set; } = new();
}
public static class RoleExtensions
{
    public static IQueryable<RoleDto> MapTo(this IQueryable<EntityWithAuditDto<Role>> entities)
    {
        var res = entities.Select(s => new RoleDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            PermissionCount = s.Entity.Permissions.Count.ToString(),

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
    public static IQueryable<RoleDto> MapToGetPermissions(this IQueryable<EntityWithAuditDto<Role>> entities)
    {
        var res = entities.Select(s => new RoleDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            PermissionCount = s.Entity.Permissions.Count.ToString(),
            Permissions = s.Entity.Permissions.Select(s => s.Value).ToList(),

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
