using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Branches.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchUpdateCommand(
    Guid Id,
    string Name,
    Address Address,
    bool IsActive) : IRequest<Result<string>>;


public sealed class BranchUpdateCommandValidator : AbstractValidator<BranchUpdateCommand>
{
    public BranchUpdateCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Branch name required")
            .MinimumLength(3).WithMessage("Branch name can not be less than 3 characters.")
            .MaximumLength(50).WithMessage("Branch name can not be greater than 50 characters.");

        RuleFor(r => r.Address.City).NotEmpty().WithMessage("City is required.");
        RuleFor(r => r.Address.District).NotEmpty().WithMessage("District is required.");
        RuleFor(r => r.Address.FullAddress).NotEmpty().WithMessage("Full address is required.");
        RuleFor(r => r.Address.PhoneNumber1).NotEmpty().WithMessage("Primary phone number is required.");
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
        Address address = request.Address;

        branch.SetName(name: name);
        branch.SetAddress(address: address);
        branch.SetStatus(isActive: request.IsActive);
        _branchRepository.Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Branch updated successfully.";
    }
}