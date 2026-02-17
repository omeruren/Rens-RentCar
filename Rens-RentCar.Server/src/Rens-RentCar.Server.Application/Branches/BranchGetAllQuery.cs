using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Branches.ValueObjects;
using Rens_RentCar.Domain.Users;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Branches;

public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchGetAllQueryResponse>>;


public sealed class BranchGetAllQueryResponse : BaseEntityDto
{
    public string Name { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

internal sealed class BranchGetAllQueryHandle(IBranchRepository _branchRepository, IUserRepository _userRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchGetAllQueryResponse>>
{
    public Task<IQueryable<BranchGetAllQueryResponse>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = _branchRepository
            .GetAll()
            .Join(_userRepository.GetAll(), m => m.CreatedBy, m => m.Id, (b, user) => new { b = b, user = user })
            .GroupJoin(_userRepository.GetAll(), m => m.b.UpdatedBy, m => m.Id, (entity, user) => new { entity = entity, user = user })
            .SelectMany(s => s.user.DefaultIfEmpty(),
            (x, user) => new
            {
                entity = x.entity,
                updatedUser = user
            })

            .Select(s => new BranchGetAllQueryResponse
            {
                Id = s.entity.b.Id,
                Name = s.entity.b.Name.Value,
                Address = s.entity.b.Address,
                CreatedAt = s.entity.b.CreatedAt,
                CreatedBy = s.entity.b.CreatedBy,

                IsActive = s.entity.b.IsActive,

                UpdatedAt = s.entity.b.UpdatedAt,
                UpdatedBy = s.entity.b.UpdatedBy,

                CreatedFullName = s.entity.user.FullName.Value,
                UpdatedFullName = s.updatedUser == null ? null : s.updatedUser.FullName.Value

            }).AsQueryable();

        return Task.FromResult(response);
    }
}