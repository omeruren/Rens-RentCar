using GenericRepository;
using Rens_RentCar.Domain.Branches;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchDeleteCommand(Guid Id) : IRequest<Result<string>>;


internal sealed class BranchDeleteCommandHandler(IBranchRepository _branchRepository, IUnitOfWork _unitOfWork) : IRequestHandler<BranchDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchDeleteCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (branch is null)
            return Result<string>.Failure("Branch not found.");

        branch.Delete();
        _branchRepository.Update(branch);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Branch deleted successfully.";
    }
}