using Rens_RentCar.Domain.Customers;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Customers;

[Permission("customer:view")]
public sealed record CustomerGetAllQuery : IRequest<IQueryable<CustomerDto>>;

internal sealed class CustomerGetAllQueryHandler(ICustomerRepository _customerRepository) : IRequestHandler<CustomerGetAllQuery, IQueryable<CustomerDto>>
{
    public Task<IQueryable<CustomerDto>> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = _customerRepository
            .GetAllWithAuditInfos()
            .MapTo();

        return Task.FromResult(response);
    }
}
