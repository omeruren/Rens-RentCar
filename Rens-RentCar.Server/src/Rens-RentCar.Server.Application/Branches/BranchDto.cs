using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;

namespace Rens_RentCar.Server.Application.Branches;

public sealed class BranchDto : BaseEntityDto
{
    public string Name { get; set; } = default!;
    public string City { get; set; } = default!;
    public string District { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
    public string PhoneNumber1 { get; set; } = default!;
    public string? PhoneNumber2 { get; set; }
    public string? Email { get; set; }
}

public static class BranchExtensions
{
    public static IQueryable<BranchDto> MapTo(this IQueryable<EntityWithAuditDto<Branch>> branches)
    {
        var result = branches
            .Select(s => new BranchDto
            {
                Id = s.Entity.Id,
                Name = s.Entity.Name.Value,
                City = s.Entity.Address.City,
                District = s.Entity.Address.District,
                FullAddress = s.Entity.Address.FullAddress,
                PhoneNumber1 = s.Entity.Address.PhoneNumber1,
                PhoneNumber2 = s.Entity.Address.PhoneNumber2,
                Email = s.Entity.Address.Email,
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