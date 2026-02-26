using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.ProtectionPackages.ValueObjects;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.ProtectionPackages;

public sealed class ProtectionPackage : BaseEntity
{
    private readonly List<ProtectionCoverage> _coverages = new();
    private ProtectionPackage() { }

    public ProtectionPackage(Name name, Price price, IsRecommended isRecommended)
    {
        SetName(name);
        SetPrice(price);
        SetIsRecommended(isRecommended);
    }

    public Name Name { get; private set; } = default!;
    public Price Price { get; private set; } = default!;
    public IsRecommended IsRecommended { get; private set; } = default!;
    public IReadOnlyCollection<ValueObjects.ProtectionCoverage> Coverages => _coverages;

    public void SetName(Name name) => Name = name;
    public void SetPrice(Price price) => Price = price;
    public void SetIsRecommended(IsRecommended isRecommended) => IsRecommended = isRecommended;
    public void SetCoverages(IEnumerable<ValueObjects.ProtectionCoverage> coverages)
    {
        _coverages.Clear();
        _coverages.AddRange(coverages);
    }
}
