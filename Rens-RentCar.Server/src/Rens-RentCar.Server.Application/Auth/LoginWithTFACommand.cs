using GenericRepository;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Auth;

public sealed record LoginWithTFACommand(
    string EmailOrUserName,
    string TFACode,
    string TfaConfirmCode) : IRequest<Result<LoginCommandResponse>>;

internal sealed class LoginWithTFACommandHandler(IUserRepository _userRepository, IJwtProvider _jwtProvider, IUnitOfWork _unitOfWork) : IRequestHandler<LoginWithTFACommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginWithTFACommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(p => p.Email.Value == request.EmailOrUserName || p.UserName.Value == request.EmailOrUserName);

        if (user is null)
            return Result<LoginCommandResponse>.Failure("Wrong User credentials.");

        if (user.TFAIscompleted is null || user.TFAExpiresDate is null || user.TFACode is null || user.TFAConfirmCode is null)
            return Result<LoginCommandResponse>.Failure("Invalid Two factor code.");

        if (user.TFAIscompleted.Value)
            return Result<LoginCommandResponse>.Failure("Invalid Two factor code.");

        if (user.TFAExpiresDate.Value < DateTimeOffset.Now)
            return Result<LoginCommandResponse>.Failure("Invalid Two factor code.");

        if (user.TFAConfirmCode.Value != request.TfaConfirmCode || user.TFACode.Value != request.TFACode)
            return Result<LoginCommandResponse>.Failure("Invalid Two factor code.");

        user.SetTFACompleted();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = await _jwtProvider.CreateJwtTokenAsync(user, cancellationToken);

        var res = new LoginCommandResponse() { Token = token };
        return res;
    }
}