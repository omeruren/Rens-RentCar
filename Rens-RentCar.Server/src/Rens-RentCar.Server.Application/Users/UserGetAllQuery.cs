using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Users;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Users;

[Permission("user:view")]

public sealed record UserGetAllQuery : IRequest<IQueryable<UserDto>>;

internal sealed class UserGetAllQueryHandler(
    IUserRepository _userRepository,
    IRoleRepository _roleRepository,
    IBranchRepository _branchRepository) : IRequestHandler<UserGetAllQuery, IQueryable<UserDto>>
{
    public Task<IQueryable<UserDto>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _userRepository.GetAllWithAuditInfos().MapTo(_roleRepository.GetAll(), _branchRepository.GetAll());

        return Task.FromResult(res);
    }
}