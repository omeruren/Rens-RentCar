using Rens_RentCar.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Auth;

public sealed record CheckForgotPasswordCodeCommand(Guid ForgotPasswordCode) : IRequest<Result<bool>>;


internal sealed class CheckForgotPasswordCodeCommandHandler(IUserRepository _userRepository) : IRequestHandler<CheckForgotPasswordCodeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckForgotPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.ForgotPasswordCode != null && u.ForgotPasswordCode.Value == request.ForgotPasswordCode && u.IsForgotPasswordCompleted.Value == false, cancellationToken);

        if (user is null)
            return Result<bool>.Failure("Invalid reset password value");

        var forgotPassDate = user.ForgotPasswordDate!.Value.AddDays(1);
        var now = DateTimeOffset.Now;

        if (forgotPassDate <= now)
            return Result<bool>.Failure("Invalid reset password value");

        return true;
    }
}