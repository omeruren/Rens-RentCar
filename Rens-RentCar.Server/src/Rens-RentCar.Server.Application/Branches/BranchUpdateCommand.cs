using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

[Permission("branch:edit")]

public sealed record BranchUpdateCommand(
    Guid Id,
    string Name,
    string City,
    string District,
    string FullAddress,
    string PhoneNumber1,
    string? PhoneNumber2,
    string? Email,
    bool IsActive) : IRequest<Result<string>>;


public sealed class BranchUpdateCommandValidator : AbstractValidator<BranchUpdateCommand>
{
    public BranchUpdateCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Branch name required")
            .MinimumLength(3).WithMessage("Branch name can not be less than 3 characters.")
            .MaximumLength(50).WithMessage("Branch name can not be greater than 50 characters.");

        RuleFor(r => r.City).NotEmpty().WithMessage("City is required.");
        RuleFor(r => r.District).NotEmpty().WithMessage("District is required.");
        RuleFor(r => r.FullAddress).NotEmpty().WithMessage("Full address is required.");
        RuleFor(r => r.PhoneNumber1).NotEmpty().WithMessage("Primary phone number is required.");
    }
}

internal sealed class BranchUpdateCommandHandler(IBranchRepository _branchRepository, IUnitOfWork _unitOfWork) : IRequestHandler<BranchUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (branch is null)
            return Result<string>.Failure("Branch not found");

        Name name = new(request.Name);

        Address address = new(request.City, request.District, request.FullAddress);
        Contact contact = new(request.PhoneNumber1, request.PhoneNumber2, request.Email);

        branch.SetName(name: name);
        branch.SetAddress(address: address);
        branch.SetStatus(isActive: request.IsActive);
        branch.SetContact(contact);
        _branchRepository.Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Branch updated successfully.";
    }
}