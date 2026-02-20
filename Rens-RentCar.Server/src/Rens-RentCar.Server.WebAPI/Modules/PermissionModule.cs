using Rens_RentCar.Server.Application.Permissions;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class PermissionModule
{
    public static void MapPermissionEndpoint(this IEndpointRouteBuilder group)
    {
        var app = group.MapGroup("/permissions")
            .WithTags("Permissions")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET 

        app.MapGet(string.Empty, async (
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(new PermissionGetAllQuery(), cancellationToken);
            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<List<string>>>();
    }
}
