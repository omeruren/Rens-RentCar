using GenericRepository;
using Rens_RentCar.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Roles;

[Permission("role:update_permissions")]
public sealed record RoleUpdatePermissionCommand(
    Guid RoleId,
    List<string> Permissions) : IRequest<Result<string>>;

internal sealed class RoleUpdatePermissionCommandHandler(IRoleRepository _roleRepository, IUnitOfWork _unitOfWork) : IRequestHandler<RoleUpdatePermissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role is null)
            return Result<string>.Failure("Role not found");

        List<Permission> permissions = request.Permissions.Select(s => new Permission(s)).ToList();

        role.SetPermissions(permissions);
        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Permissions has been updated.";
    }
}