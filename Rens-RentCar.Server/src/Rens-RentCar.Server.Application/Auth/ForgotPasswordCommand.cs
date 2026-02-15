using FluentValidation;
using Rens_RentCar.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Auth;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Result<string>>;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(customer => customer.Email)
            .NotEmpty()
                .WithMessage("Email address is required.")
            .EmailAddress()
                .WithMessage("A valid email address is required.");
    }
}

internal sealed class ForgotPasswordCommandHandler(IUserRepository _userRepository) : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.Email.Value == request.Email, cancellationToken);
        if (user is null)
            return Result<string>.Failure("User not found.");

        // TODO : send reset password email 

        return "A password reset email has been sent. Please check your email account.";
    }
}