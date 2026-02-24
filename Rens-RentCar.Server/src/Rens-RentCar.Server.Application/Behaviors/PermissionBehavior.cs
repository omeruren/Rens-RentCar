using Rens_RentCar.Server.Application.Services;
using System.Reflection;
using TS.MediatR;

public sealed class PermissionBehavior<TRequest, TResponse>(
    IClaimContext userContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        var attr = request.GetType().GetCustomAttribute<PermissionAttribute>(inherit: true);

        if (attr is null) return await next();

        var userId = userContext.GetUserId();

        //var user = await userRepository.FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);
        //if (user is null)
        //{
        //    throw new ArgumentException("User not found");
        //}

        // Check if Permission String is exists
        //if (!string.IsNullOrEmpty(attr.Permission))
        //{
        //    var hasPermission = user.Permissions.Any(p => p.Name == attr.Permission);
        //    if (!hasPermission)
        //    {
        //        throw new AuthorizationException($"You do not have '{attr.Permission}' Permission.");
        //    }
        //}

        // Check is there Admin role if Permission String is not exists
        //else if (!user.IsAdmin.Value)
        //{
        //    throw new AuthorizationException("Admin Role required for this process.");
        //}

        return await next();
    }
}

public sealed class PermissionAttribute : Attribute
{
    public string? Permission { get; }

    public PermissionAttribute()
    {
    }

    public PermissionAttribute(string permission)
    {
        Permission = permission;
    }
}

public sealed class AuthorizationException : Exception
{
    public AuthorizationException() : base("You do not have any permission.")
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }
}

// Example usage

/*
 * [Permission]
public sealed record DeveloperCreateCommand(
    string Name
) : IRequest<Result<string>>;

[Permission("permission.create")]
public sealed record DeveloperCreateCommand(
    string Name
) : IRequest<Result<string>>;
*/