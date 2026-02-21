using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Branches;

public sealed class Branch : BaseEntity
{
    private Branch() { }
    public Branch(Name name, Address address, Contact contact, bool isActive)
    {
        SetName(name);
        SetAddress(address);
        SetStatus(isActive);
        SetContact(contact);
    }

    public void SetContact(Contact contact) => Contact = contact;

    public Name Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public Contact Contact { get; private set; } = default!;

    public void SetName(Name name) => Name = name;
    public void SetAddress(Address address) => Address = address;
}
