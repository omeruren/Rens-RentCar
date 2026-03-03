using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Reservations;

public interface IReservationRepository : IAuditableRepository<Reservation>
{
}
