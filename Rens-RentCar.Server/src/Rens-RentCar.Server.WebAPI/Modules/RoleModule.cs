using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Server.Application.Roles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class RoleModule
{
    public static void MapRoleEndPoint(this IEndpointRouteBuilder group)
    {
        var app = group.MapGroup("/roles")
            .WithTags("Roles")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET BY ID

        app.MapGet("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(new RoleGetQuery(id), cancellationToken);

            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<RoleDto>>();

        // CREATE ROLE

        app.MapPost(string.Empty, async (
            RoleCreateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(request, cancellationToken);

            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();

        // UPDATE ROLE

        app.MapPut(string.Empty, async (
            RoleUpdateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(request, cancellationToken);

            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();

        // UPDATE ROLE - SET PERMISSION

        app.MapPut("update-permissions", async (
            RoleUpdatePermissionCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(request, cancellationToken);

            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();

        // DELETE ROLE

        app.MapDelete("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(new RoleDeleteCommand(id), cancellationToken);
            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();
    }
}
