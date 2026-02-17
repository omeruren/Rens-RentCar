using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Branches.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchCreateCommand(
    string Name,
    Address Address) : IRequest<Result<string>>;


public sealed class BranchCreateCommandValidator : AbstractValidator<BranchCreateCommand>
{
    public BranchCreateCommandValidator()
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

internal sealed class BrachCreateCommandHandler(IBranchRepository _branchRepository, IUnitOfWork _unitOfWork) : IRequestHandler<BranchCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchCreateCommand request, CancellationToken cancellationToken)
    {
        var isBranchAlreadyTaken = await _branchRepository.AnyAsync(x => x.Name.Value == request.Name, cancellationToken);

        if (isBranchAlreadyTaken)
            return Result<string>.Failure("This branch name is already taken by someone else.");

        Name name = new(request.Name);
        Address address = request.Address;

        Branch branch = new(name, address);

        _branchRepository.Add(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Branch Created successfully.";
    }
}