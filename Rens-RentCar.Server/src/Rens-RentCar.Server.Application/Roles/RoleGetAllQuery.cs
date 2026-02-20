using Rens_RentCar.Domain.Roles;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Roles;

public sealed record RoleGetAllQuery : IRequest<IQueryable<RoleDto>>;

internal sealed class RoleGetAllQueryHandler(IRoleRepository _roleRepository) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleDto>>
{
    public Task<IQueryable<RoleDto>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _roleRepository
            .GetAllWithAuditInfos()
            .MapTo();

        return Task.FromResult(res);

    }
}