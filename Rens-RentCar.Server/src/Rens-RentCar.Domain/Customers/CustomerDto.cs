using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Customers;

public sealed class CustomerDto : BaseEntityDto
{
    public string NationalId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateOnly BirthDate { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateOnly DrivingLicenseIssueDate { get; set; }
    public string FullAddress { get; set; } = default!;
}

public static class CustomerExtensions
{
    public static IQueryable<CustomerDto> MapTo(this IQueryable<EntityWithAuditDto<Customer>> customers)
    {
        var result = customers
            .Select(s => new CustomerDto
            {
                Id = s.Entity.Id,
                NationalId = s.Entity.NationalId.Value,
                FirstName = s.Entity.FirstName.Value,
                LastName = s.Entity.LastName.Value,
                FullName = s.Entity.FullName.Value,
                BirthDate = s.Entity.BirthDate.Value,
                PhoneNumber = s.Entity.PhoneNumber.Value,
                Email = s.Entity.Email.Value,
                DrivingLicenseIssueDate = s.Entity.DrivingLicenseIssueDate.Value,
                FullAddress = s.Entity.FullAddress.Value,
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
