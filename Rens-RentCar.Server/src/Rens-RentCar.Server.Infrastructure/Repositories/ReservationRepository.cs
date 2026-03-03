using Rens_RentCar.Domain.Reservations;
using Rens_RentCar.Server.Infrastructure.Abstractions;
using Rens_RentCar.Server.Infrastructure.Context;

namespace Rens_RentCar.Server.Infrastructure.Repositories;

internal sealed class ReservationRepository : AuditableRepository<Reservation, ApplicationDbContext>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
