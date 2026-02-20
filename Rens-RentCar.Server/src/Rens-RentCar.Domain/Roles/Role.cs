using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Roles;

public sealed class Role : BaseEntity
{
    public Name Name { get; private set; } = default!;

}
