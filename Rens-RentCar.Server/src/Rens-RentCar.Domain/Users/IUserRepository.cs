using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Users;

public interface IUserRepository : IAuditableRepository<User>
{
}
