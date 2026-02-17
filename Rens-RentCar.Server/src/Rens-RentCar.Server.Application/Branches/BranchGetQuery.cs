using Rens_RentCar.Domain.Branches;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchGetQuery(Guid Id) : IRequest<Result<Branch>>;

internal sealed class BranchGetQueryHandler(IBranchRepository _branchRepository) : IRequestHandler<BranchGetQuery, Result<Branch>>
{
    public async Task<Result<Branch>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (branch is null)
            return Result<Branch>.Failure("Branch not found.");

        return branch;
    }
}