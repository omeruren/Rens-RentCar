using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Customers;
using Rens_RentCar.Domain.Customers.ValueObjects;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Customers;

[Permission("customer:edit")]
public sealed record CustomerUpdateCommand(
    Guid Id,
    string NationalId,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string PhoneNumber,
    string Email,
    DateOnly DrivingLicenseIssueDate,
    string FullAddress,
    bool IsActive) : IRequest<Result<string>>;

public sealed class CustomerUpdateCommandValidator : AbstractValidator<CustomerUpdateCommand>
{
    public CustomerUpdateCommandValidator()
    {
        RuleFor(r => r.NationalId)
            .NotEmpty().WithMessage("National ID is required.");

        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(r => r.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.");

        RuleFor(r => r.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(r => r.DrivingLicenseIssueDate)
            .NotEmpty().WithMessage("Driving license issue date is required.");

        RuleFor(r => r.FullAddress)
            .NotEmpty().WithMessage("Full address is required.");
    }
}

internal sealed class CustomerUpdateCommandHandler(ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IRequestHandler<CustomerUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (customer is null)
            return Result<string>.Failure("Customer not found.");

        customer.SetNationalId(new NationalId(request.NationalId));
        customer.SetFirstName(new FirstName(request.FirstName));
        customer.SetLastName(new LastName(request.LastName));
        customer.SetFullName();
        customer.SetBirthDate(new(request.BirthDate));
        customer.SetPhoneNumber(new PhoneNumber(request.PhoneNumber));
        customer.SetEmail(new Email(request.Email));
        customer.SetDrivingLicenceIssuanceDate(new DrivingLicenseIssueDate(request.DrivingLicenseIssueDate));
        customer.SetFullAddress(new FullAddress(request.FullAddress));
        customer.SetStatus(request.IsActive);

        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Customer updated successfully.";
    }
}
