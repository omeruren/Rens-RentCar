using GenericRepository;
using Rens_RentCar.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Users;

[Permission("user:delete")]

public sealed record UserDeleteCommand(Guid Id) : IRequest<Result<string>>;


internal sealed class UserDeleteCommandHandler(IUserRepository _userRepository, IUnitOfWork _unitOfWork) : IRequestHandler<UserDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return Result<string>.Failure("User not found.");

        if (user.UserName.Value == "admin")
            return Result<string>.Failure("Do not cross the line!!!");

        user.Delete();
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "User deleted successfully.";
    }
}