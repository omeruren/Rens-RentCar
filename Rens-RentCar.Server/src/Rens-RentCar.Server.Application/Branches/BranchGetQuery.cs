using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchGetQuery(Guid Id) : IRequest<Result<BranchDto>>;

internal sealed class BranchGetQueryHandler(IBranchRepository _branchRepository, IUserRepository _userRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.
            Where(x => x.Id == request.Id)
             .Join(_userRepository.GetAll(), m => m.CreatedBy, m => m.Id, (b, user) => new { b = b, user = user })
            .GroupJoin(_userRepository.GetAll(), m => m.b.UpdatedBy, m => m.Id, (entity, user) => new { entity = entity, user = user })
            .SelectMany(s => s.user.DefaultIfEmpty(),
            (x, user) => new
            {
                entity = x.entity,
                updatedUser = user
            })

            .Select(s => new BranchDto
            {
                Id = s.entity.b.Id,
                Name = s.entity.b.Name.Value,
                Address = s.entity.b.Address,
                CreatedAt = s.entity.b.CreatedAt,
                CreatedBy = s.entity.b.CreatedBy,

                IsActive = s.entity.b.IsActive,

                UpdatedAt = s.entity.b.UpdatedAt,
                UpdatedBy = s.entity.b.UpdatedBy == null ? null : s.entity.b.UpdatedBy.Value,

                CreatedFullName = s.entity.user.FullName.Value,
                UpdatedFullName = s.updatedUser == null ? null : s.updatedUser.FullName.Value

            }).FirstOrDefaultAsync(cancellationToken);
        if (branch is null)
            return Result<BranchDto>.Failure("Branch not found.");

        return branch;
    }
}