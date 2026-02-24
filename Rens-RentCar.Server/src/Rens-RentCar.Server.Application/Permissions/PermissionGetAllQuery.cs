using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Permissions;

//[Permission("permission:view")]
public sealed record PermissionGetAllQuery : IRequest<Result<List<string>>>;

internal sealed class PermissionGetAllQueryHandler(PermissionService _permissionService) : IRequestHandler<PermissionGetAllQuery, Result<List<string>>>
{
    public Task<Result<List<string>>> Handle(PermissionGetAllQuery request, CancellationToken cancellationToken)
    {
        var list = _permissionService.GetPermissions();

        return Task.FromResult(Result<List<string>>.Succeed(list));
    }
}