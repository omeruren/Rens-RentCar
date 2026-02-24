using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Domain.Users.ValueObjects;
using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Users;

public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId) : IRequest<Result<string>>;


public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(u => u.UserName).NotEmpty().WithMessage("User name is required.");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
    }
}

internal sealed class UserCreateCommandHandler(
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork,
    IClaimContext _claimContext) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var isEmailExists = await _userRepository.AnyAsync(u => u.Email.Value == request.Email, cancellationToken);

        if (isEmailExists)
            return Result<string>.Failure("Email address is already taken before by someone else");

        var isUserNameExists = await _userRepository.AnyAsync(u => u.UserName.Value == request.UserName, cancellationToken);

        if (isUserNameExists)
            return Result<string>.Failure("User name is already taken before by someone else");

        FirstName firstName = new(request.FirstName);
        LastName lastName = new(request.LastName);
        Email email = new(request.Email);
        UserName userName = new(request.UserName);
        Password password = new("Password123!");
        IdentityId branchId =
            request.BranchId is not null
            ? new(request.BranchId.Value)
            : new(_claimContext.GetBranchId());

        IdentityId roleId = new(request.RoleId);

        User user = new(firstName, lastName, email, userName, password, branchId, roleId);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "User created successfully.";
    }
}