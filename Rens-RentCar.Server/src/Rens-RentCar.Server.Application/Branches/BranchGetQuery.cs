using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Branches;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

[Permission("branch:view")]
public sealed record BranchGetQuery(Guid Id) : IRequest<Result<BranchDto>>;

internal sealed class BranchGetQueryHandler(IBranchRepository _branchRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetAllWithAuditInfos()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (branch is null)
            return Result<BranchDto>.Failure("Branch not found.");

        return branch;
    }
}