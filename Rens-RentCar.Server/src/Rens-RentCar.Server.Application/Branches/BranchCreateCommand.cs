using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

[Permission("branch:create")]
public sealed record BranchCreateCommand(
    string Name,
    string City,
    string District,
    string FullAddress,
    string PhoneNumber1,
    string? PhoneNumber2,
    string? Email,
    bool IsActive) : IRequest<Result<string>>;



public sealed class BranchCreateCommandValidator : AbstractValidator<BranchCreateCommand>
{
    public BranchCreateCommandValidator()
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

internal sealed class BrachCreateCommandHandler(IBranchRepository _branchRepository, IUnitOfWork _unitOfWork) : IRequestHandler<BranchCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchCreateCommand request, CancellationToken cancellationToken)
    {
        var isBranchAlreadyTaken = await _branchRepository.AnyAsync(x => x.Name.Value == request.Name, cancellationToken);

        if (isBranchAlreadyTaken)
            return Result<string>.Failure("This branch name is already taken by someone else.");

        Name name = new(request.Name);


        Address address = new(request.City, request.District, request.FullAddress);
        Contact contact = new(request.PhoneNumber1, request.PhoneNumber2, request.Email);
        Branch branch = new(name, address, contact, request.IsActive);

        _branchRepository.Add(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Branch Created successfully.";
    }
}