using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Roles;

[Permission("role:view")]

public sealed record RoleGetQuery(Guid Id) : IRequest<Result<RoleDto>>;

internal sealed class RoleGetQueryHandler(IRoleRepository _roleRepository) : IRequestHandler<RoleGetQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(RoleGetQuery request, CancellationToken cancellationToken)
    {
        var res = await _roleRepository
            .GetAllWithAuditInfos()
            .MapToGetPermissions()
            .Where(r => r.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<RoleDto>.Failure("Role nor found.");

        return res;
    }
}