using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Roles;
using System.Reflection;

namespace Rens_RentCar.Server.Application.Services;

public sealed class PermissionService
{
    public List<string> GetPermissions()
    {
        var permissions = new HashSet<string>();

        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var permissionAttr = type.GetCustomAttribute<PermissionAttribute>();

            if (permissionAttr is not null && !string.IsNullOrEmpty(permissionAttr.Permission))
                permissions.Add(permissionAttr.Permission);
        }

        return permissions.ToList();
    }
}


public sealed class PermissionCleanerService(IRoleRepository _roleRepository, PermissionService _permissionService, IUnitOfWork _unitOfWork)
{
    public async Task CleanRemovedPermissionFromRoleAsync(CancellationToken cancellationToken = default)
    {
        var currentPermissions = _permissionService.GetPermissions();
        var roles = await _roleRepository.GetAllWithTracking().ToListAsync(cancellationToken);
        foreach (var role in roles)
        {
            var currentPermissionsForRole = role.Permissions.Select(s => s.Value).ToList();
            var filteredPermissions = currentPermissionsForRole.Where(p => currentPermissions.Contains(p)).ToList();
            var permissions = filteredPermissions.Select(s => new Permission(s)).ToList();
            role.SetPermissions(permissions);
        }
        _roleRepository.UpdateRange(roles);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}