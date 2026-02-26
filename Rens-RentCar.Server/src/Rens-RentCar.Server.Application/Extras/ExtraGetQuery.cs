using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Extras;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Extras;

[Permission("extra:view")]
public sealed record ExtraGetQuery(Guid Id) : IRequest<Result<ExtraDto>>;
internal sealed class ExtraGetQueryHandler(
    IExtraRepository _extraRepository) : IRequestHandler<ExtraGetQuery, Result<ExtraDto>>
{
    public async Task<Result<ExtraDto>> Handle(ExtraGetQuery request, CancellationToken cancellationToken)
    {
        var res = await _extraRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<ExtraDto>.Failure("Extra not found.");

        return res;
    }
}