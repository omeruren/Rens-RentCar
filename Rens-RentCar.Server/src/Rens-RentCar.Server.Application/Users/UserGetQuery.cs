using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Users;

public sealed record UserGetQuery(Guid Id) : IRequest<Result<UserDto>>;

internal sealed class UserGetQueryHandler(
    IUserRepository _userRepository,
    IRoleRepository _roleRepository,
    IBranchRepository _branchRepository) : IRequestHandler<UserGetQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UserGetQuery request, CancellationToken cancellationToken)
    {
        var res = await _userRepository
            .GetAllWithAuditInfos()
            .MapTo(_roleRepository.GetAll(), _branchRepository.GetAll()).Where(u => u.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (res is null)
            return Result<UserDto>.Failure("User not found");

        return res;
    }
}