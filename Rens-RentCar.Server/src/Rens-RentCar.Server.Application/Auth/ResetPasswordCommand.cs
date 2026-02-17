using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.LoginTokens;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Auth;

public sealed record ResetPasswordCommand(
    Guid ForgotPasswordCode,
    string NewPassword,
    bool LogoutAllDevices) : IRequest<Result<string>>;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(r => r.NewPassword).NotEmpty().WithMessage("Password is required");
    }
}

internal sealed class ResetPasswordCommandHandler(IUserRepository _userRepository, IUnitOfWork _unitOfWork, ILoginTokenRepository _LoginTokenRepository) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.ForgotPasswordCode != null && u.ForgotPasswordCode.Value == request.ForgotPasswordCode && u.IsForgotPasswordCompleted.Value == false, cancellationToken);

        if (user is null)
            return Result<string>.Failure("Invalid reset password value");

        var forgotPassDate = user.ForgotPasswordDate!.Value.AddDays(1);
        var now = DateTimeOffset.Now;

        if (forgotPassDate <= now)
            return Result<string>.Failure("Invalid reset password value");

        Password password = new(request.NewPassword);
        user.SetPassword(password);

        _userRepository.Update(user);
        if (request.LogoutAllDevices)
        {

            var loginTokens = await _LoginTokenRepository.Where(p => p.UserId == user.Id && p.IsActive.Value == true).ToListAsync(cancellationToken);

            foreach (var item in loginTokens)
                item.SetIsActive(new(false));

            _LoginTokenRepository.UpdateRange(loginTokens);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Your password has been successfully reset. You can log in with your new password.";
    }
}