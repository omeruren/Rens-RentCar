using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Roles;

namespace Rens_RentCar.Domain.Users;

public sealed class UserDto : BaseEntityDto
{
    public string FisrtName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public string RoleName { get; set; } = default!;

}

public static class UserExtensions
{
    public static IQueryable<UserDto> MapTo(
        this IQueryable<EntityWithAuditDto<User>> users,
        IQueryable<Role> roles,
        IQueryable<Branch> branches)
    {
        var res = users.
            Join(roles, m => m.Entity.RoleId, m => m.Id, (e, role) => new { e.Entity, e.CreatedUser, e.UpdatedUser, Role = role })
            .Join(branches, m => m.Entity.BranchId, m => m.Id, (entity, branch) => new { entity.Entity, entity.CreatedUser, entity.UpdatedUser, entity.Role, Branch = branch })
            .Select(s => new UserDto
            {
                Id = s.Entity.Id,
                FisrtName = s.Entity.FirstName.Value,
                LastName = s.Entity.LastName.Value,
                FullName = s.Entity.FullName.Value,
                Email = s.Entity.Email.Value,
                UserName = s.Entity.UserName.Value,
                RoleName = s.Role.Name.Value,
                BranchName = s.Branch.Name.Value,


                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy,

                IsActive = s.Entity.IsActive,

                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy == null ? null : s.Entity.UpdatedBy.Value,

                CreatedFullName = s.CreatedUser.FullName.Value,
                UpdatedFullName = s.UpdatedUser == null ? null : s.UpdatedUser.FullName.Value
            });

        return res;
    }
}