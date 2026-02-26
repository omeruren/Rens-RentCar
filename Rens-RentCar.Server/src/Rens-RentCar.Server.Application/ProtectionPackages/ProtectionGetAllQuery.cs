using Rens_RentCar.Domain.ProtectionPackages;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.ProtectionPackages;

[Permission("protection:view")]
public sealed record ProtectionGetAllQuery : IRequest<IQueryable<ProtectionDto>>;

internal sealed class ProtectionGetAllQueryHandler(IProtectionPackageRepository _protectionRepository) : IRequestHandler<ProtectionGetAllQuery, IQueryable<ProtectionDto>>
{
    public Task<IQueryable<ProtectionDto>> Handle(ProtectionGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _protectionRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .AsQueryable();

        return Task.FromResult(res);
    }
}
