using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Categories;

public sealed class Category : BaseEntity
{
    private Category() { }

    public Category(Name name, bool isActive)
    {
        SetName(name);
        SetStatus(isActive);
    }

    public Name Name { get; private set; } = default!;

    public void SetName(Name name) => Name = name;
}
