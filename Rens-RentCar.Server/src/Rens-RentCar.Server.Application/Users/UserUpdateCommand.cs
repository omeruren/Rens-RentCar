using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Domain.Users.ValueObjects;
using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Users;

[Permission("user:edit")]

public sealed record UserUpdateCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId,
    bool IsActive) : IRequest<Result<string>>;

public sealed class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(u => u.UserName).NotEmpty().WithMessage("User name is required.");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required");
    }
}

internal sealed class UserUpdateCommandHandler(
    IUserRepository _userRepository,
    IClaimContext _claimContext,
    IUnitOfWork _unitOfWork) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return Result<string>.Failure("User not found.");

        if (user.Email.Value != request.Email)
        {
            var isEmailExists = await _userRepository.AnyAsync(u => u.Email.Value == request.Email, cancellationToken);
            if (isEmailExists)
                return Result<string>.Failure("Email address is already taken before by someone else.");
        }

        if (user.UserName.Value != request.UserName)
        {
            var isUserNameIsExists = await _userRepository.AnyAsync(u => u.UserName.Value == request.UserName, cancellationToken);
            if (isUserNameIsExists)
                return Result<string>.Failure("User name is already taken before by someone else.");
        }

        FirstName firstName = new(request.FirstName);
        LastName lastName = new(request.LastName);
        Email email = new(request.Email);
        UserName userName = new(request.UserName);
        IdentityId branchId = request.BranchId is null
            ? new(_claimContext.GetBranchId())
            : new(request.BranchId.Value);
        IdentityId roleId = new(request.RoleId);

        user.SetFirstName(firstName);
        user.SetLastName(lastName);
        user.SetFullName();
        user.SetEmail(email);
        user.SetUserName(userName);
        user.SetBranchId(branchId);
        user.SetRoleId(roleId);
        user.SetStatus(request.IsActive);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "User Updated successfully.";
    }
}