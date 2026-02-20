using GenericRepository;
using Rens_RentCar.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Roles;

[Permission("role:delete")]

public sealed record RoleDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class RoleDeleteCommandHandler(IRoleRepository _roleRepository, IUnitOfWork _unitOfWork) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        Role? role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            return Result<string>.Failure("Role not found.");

        role.Delete();
        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return "Role Deleted successfully.";
    }
}