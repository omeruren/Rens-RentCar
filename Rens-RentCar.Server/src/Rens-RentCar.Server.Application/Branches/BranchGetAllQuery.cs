using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Users;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchDto>>;

internal sealed class BranchGetAllQueryHandle(IBranchRepository _branchRepository, IUserRepository _userRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchDto>>
{
    public Task<IQueryable<BranchDto>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = _branchRepository
            .GetAll()
            .MapTo(_userRepository.GetAll());

        return Task.FromResult(response);
    }
}