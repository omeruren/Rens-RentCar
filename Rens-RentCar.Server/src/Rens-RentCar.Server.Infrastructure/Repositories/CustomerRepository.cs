using Rens_RentCar.Domain.Customers;
using Rens_RentCar.Server.Infrastructure.Abstractions;
using Rens_RentCar.Server.Infrastructure.Context;

namespace Rens_RentCar.Server.Infrastructure.Repositories;

internal sealed class CustomerRepository : AuditableRepository<Customer, ApplicationDbContext>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
}
