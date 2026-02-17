using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches.ValueObjects;

namespace Rens_RentCar.Domain.Branches;

public sealed class Branch : BaseEntity
{
    private Branch() { }
    public Branch(Name name, Address address)
    {
        SetName(name);
        SetAddress(address);
    }

    public Name Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;

    public void SetName(Name name) => Name = name;
    public void SetAddress(Address address) => Address = address;
}
