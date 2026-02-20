using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Roles;

public sealed class Role : BaseEntity, IAggregate
{
    private readonly List<Permission> _permissions = new();
    private Role()
    {

    }

    public Role(Name name, bool IsActive)
    {
        SetName(name);
        SetStatus(IsActive);
    }


    public Name Name { get; private set; } = default!;
    public IReadOnlyCollection<Permission> Permissions => _permissions;
    public void SetName(Name name) => Name = name;

    public void SetPermissions(IEnumerable<Permission> permissions)
    {
        _permissions.Clear();
        _permissions.AddRange(permissions);
    }
}

public sealed record Permission(string Value);