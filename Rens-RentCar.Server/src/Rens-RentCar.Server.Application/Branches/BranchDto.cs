using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Branches.ValueObjects;
using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Server.Application.Branches;

public sealed class BranchDto : BaseEntityDto
{
    public string Name { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

public static class BranchExtensions
{
    public static IQueryable<BranchDto> MapTo(this IQueryable<Branch> branches, IQueryable<User> users)
    {
        var result = branches.ApplyAuditDto(users)
            .Select(s => new BranchDto
            {
                Id = s.Entity.Id,
                Name = s.Entity.Name.Value,
                Address = s.Entity.Address,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy,

                IsActive = s.Entity.IsActive,

                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy == null ? null : s.Entity.UpdatedBy.Value,

                CreatedFullName = s.CreatedUser.FullName.Value,
                UpdatedFullName = s.UpdatedUser == null ? null : s.UpdatedUser.FullName.Value

            }).AsQueryable();

        return result;
    }
}