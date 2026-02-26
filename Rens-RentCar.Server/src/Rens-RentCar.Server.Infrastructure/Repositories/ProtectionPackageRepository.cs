using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Server.Infrastructure.Abstractions;
using Rens_RentCar.Server.Infrastructure.Context;

namespace Rens_RentCar.Server.Infrastructure.Repositories;

internal sealed class ProtectionPackageRepository : AuditableRepository<ProtectionPackage, ApplicationDbContext>, IProtectionPackageRepository
{
    public ProtectionPackageRepository(ApplicationDbContext context) : base(context)
    {
    }
}