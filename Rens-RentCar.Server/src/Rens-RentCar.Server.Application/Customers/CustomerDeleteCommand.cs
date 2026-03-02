using GenericRepository;
using Rens_RentCar.Domain.Customers;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Customers;

[Permission("customer:delete")]
public sealed record CustomerDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class CustomerDeleteCommandHandler(ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IRequestHandler<CustomerDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (customer is null)
            return Result<string>.Failure("Customer not found.");

        customer.Delete();
        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Customer deleted successfully.";
    }
}
