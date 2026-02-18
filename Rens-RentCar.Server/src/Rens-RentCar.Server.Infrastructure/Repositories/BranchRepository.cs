using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Server.Infrastructure.Abstractions;
using Rens_RentCar.Server.Infrastructure.Context;

namespace Rens_RentCar.Server.Infrastructure.Repositories;

internal sealed class BranchRepository : AuditableRepository<Branch, ApplicationDbContext>, IBranchRepository
{
    public BranchRepository(ApplicationDbContext context) : base(context)
    {
    }
}
