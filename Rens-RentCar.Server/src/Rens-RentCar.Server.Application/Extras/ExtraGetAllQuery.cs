using Rens_RentCar.Domain.Extras;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Extras;

[Permission("extra:view")]
public sealed record ExtraGetAllQuery : IRequest<IQueryable<ExtraDto>>;
internal sealed class ExtraGetAllQueryHandler(
    IExtraRepository _extraRepository) : IRequestHandler<ExtraGetAllQuery, IQueryable<ExtraDto>>
{
    public Task<IQueryable<ExtraDto>> Handle(ExtraGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _extraRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .AsQueryable();

        return Task.FromResult(res);
    }
}