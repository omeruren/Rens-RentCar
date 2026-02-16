using FluentValidation;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Auth;

public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<string>>;


public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(r => r.EmailOrUserName).NotEmpty().WithMessage("Email or username required.");
        RuleFor(r => r.Password).NotEmpty().WithMessage("Password required.");
    }
}

internal sealed class LoginCommandHandler(IUserRepository _userRepository, IJwtProvider _jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName.Value == request.EmailOrUserName || u.Email.Value == request.EmailOrUserName);

        if (user is null)
            return Result<string>.Failure("Incorrect user credentials.");

        var isPasswordsMatched = user.VerifyPasswordHash(request.Password);

        if (!isPasswordsMatched)
            return Result<string>.Failure("Incorrect user credentials.");

        var token = await _jwtProvider.CreateJwtTokenAsync(user, cancellationToken);

        return token;
    }
}