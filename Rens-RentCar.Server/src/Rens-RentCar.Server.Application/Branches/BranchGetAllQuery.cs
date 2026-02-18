using Rens_RentCar.Domain.Branches;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchDto>>;

internal sealed class BranchGetAllQueryHandle(IBranchRepository _branchRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchDto>>
{
    public Task<IQueryable<BranchDto>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = _branchRepository
            .GetAllWithAuditInfos()
            .MapTo();

        return Task.FromResult(response);
    }
}