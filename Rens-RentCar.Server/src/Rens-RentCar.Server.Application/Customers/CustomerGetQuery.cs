using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Customers;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Customers;

[Permission("customer:view")]
public sealed record CustomerGetQuery(Guid Id) : IRequest<Result<CustomerDto>>;

internal sealed class CustomerGetQueryHandler(ICustomerRepository _customerRepository) : IRequestHandler<CustomerGetQuery, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CustomerGetQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAllWithAuditInfos()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
            return Result<CustomerDto>.Failure("Customer not found.");

        return customer;
    }
}
