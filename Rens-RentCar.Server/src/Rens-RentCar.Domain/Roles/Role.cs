using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Roles;

public sealed class Role : BaseEntity
{
    private Role()
    {

    }

    public Role(Name name, bool IsActive)
    {
        SetName(name);
        SetStatus(IsActive);
    }


    public Name Name { get; private set; } = default!;

    public void SetName(Name name) => Name = name;
}
