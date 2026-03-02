using Rens_RentCar.Domain.Abstraction;

namespace Rens_RentCar.Domain.Customers;

public interface ICustomerRepository : IAuditableRepository<Customer>
{
}
