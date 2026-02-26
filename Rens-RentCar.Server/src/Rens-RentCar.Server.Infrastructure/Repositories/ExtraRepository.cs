using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Server.Infrastructure.Abstractions;
using Rens_RentCar.Server.Infrastructure.Context;

namespace Rens_RentCar.Server.Infrastructure.Repositories;

internal sealed class ExtraRepository : AuditableRepository<Extra, ApplicationDbContext>, IExtraRepository
{
    public ExtraRepository(ApplicationDbContext context) : base(context)
    {
    }
}
