using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Auth;

public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string? Token { get; set; }
    public string? TFACode { get; set; }
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(r => r.EmailOrUserName).NotEmpty().WithMessage("Email or username required.");
        RuleFor(r => r.Password).NotEmpty().WithMessage("Password required.");
    }
}

internal sealed class LoginCommandHandler(IUserRepository _userRepository, IJwtProvider _jwtProvider, IMailService _mailService, IUnitOfWork _unitOfWork) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName.Value == request.EmailOrUserName || u.Email.Value == request.EmailOrUserName);

        if (user is null)
            return Result<LoginCommandResponse>.Failure("Incorrect user credentials.");

        var isPasswordsMatched = user.VerifyPasswordHash(request.Password);

        if (!isPasswordsMatched)
            return Result<LoginCommandResponse>.Failure("Incorrect user credentials.");

        if (!user.TFAStatus.Value)
        {
            var token = await _jwtProvider.CreateJwtTokenAsync(user, cancellationToken);
            var res = new LoginCommandResponse() { Token = token };

            return res;
        }
        else
        {
            user.CreateTFACode();

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            string to = user.Email.Value;
            string subject = "Two Factor Code";
            string body = @$"To log in to the application, please enter the following code. <b>This code is available for 5 minutes </b> <br/> <p><h3>{user.TFAConfirmCode!.Value}</h3></p>";

            await _mailService.SendAsync(to, subject, body, cancellationToken);

            var res = new LoginCommandResponse { TFACode = user.TFACode!.Value };
            return res;
        }
    }
}