using System.Reflection;

namespace Rens_RentCar.Server.Application.Services;

internal sealed class PermissionService
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
