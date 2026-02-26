using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.ProtectionPackages;

[Permission("protection:view")]
public sealed record ProtectionGetQuery(Guid Id) : IRequest<Result<ProtectionDto>>;


internal sealed class ProtectionGetQueryHandler(IProtectionPackageRepository _protectionRepository) : IRequestHandler<ProtectionGetQuery, Result<ProtectionDto>>
{
    public async Task<Result<ProtectionDto>> Handle(ProtectionGetQuery request, CancellationToken cancellationToken)
    {
        var protection = await _protectionRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (protection is null)
            return Result<ProtectionDto>.Failure("Protection package not found.");

        return protection;
    }
}
