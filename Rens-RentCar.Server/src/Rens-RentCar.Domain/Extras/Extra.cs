using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Shared;
using Rens_RentCar.Domain.Extras.ValueObjects;

namespace Rens_RentCar.Domain.Extras;

public sealed class Extra : BaseEntity
{
    private Extra() { }

    public Extra(Name name, Price price, Description description, bool isActive)
    {
        SetName(name);
        SetPrice(price);
        SetDescription(description);
        SetStatus(isActive);
    }

    public Name Name { get; private set; } = default!;
    public Price Price { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public void SetName(Name name) => Name = name;
    public void SetPrice(Price price) => Price = price;
    public void SetDescription(Description description) => Description = description;
}
