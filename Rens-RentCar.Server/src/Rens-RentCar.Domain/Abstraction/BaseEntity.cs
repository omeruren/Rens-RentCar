using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Domain.Abstraction;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = new IdentityId(Guid.CreateVersion7());
        IsActive = true;
    }
    public IdentityId Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public IdentityId CreatedBy { get; private set; } = default!;

    public string CreatedFullName => GetCreatedFullName();
    public string? UpdatedFullName => GetUpdatedFullName();


    public IdentityId? UpdatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; }
    public IdentityId? DeletedBy { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsActive { get; private set; }


    public void SetStatus(bool isActive) => IsActive = isActive;
    public void Delete() => IsDeleted = true;
    private string GetCreatedFullName()
    {
        HttpContextAccessor httpContextAccessor = new();
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
            return string.Empty;
        using var scope = httpContext.RequestServices.CreateScope();
        var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;

        if (userRepository is null) return string.Empty;

        var fullName = userRepository.FirstOrDefault(x => x.Id == CreatedBy).FullName;

        if (fullName == null) return string.Empty;

        return fullName.Value;
    }
    private string? GetUpdatedFullName()
    {
        if (UpdatedBy is not null)
        {

            HttpContextAccessor httpContextAccessor = new();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
                return string.Empty;
            using var scope = httpContext.RequestServices.CreateScope();
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;

            if (userRepository is null) return string.Empty;

            var fullName = userRepository.FirstOrDefault(x => x.Id == UpdatedBy).FullName;

            if (fullName == null) return string.Empty;

            return fullName.Value;
        }
        return null;
    }

}
public sealed record IdentityId(Guid Value)
{
    public static implicit operator Guid(IdentityId id) => id.Value;
    public static implicit operator string(IdentityId id) => id.Value.ToString();
}
