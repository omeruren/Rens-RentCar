using Rens_RentCar.Server.Application.Users;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class UserModule
{
    public static void MapUserEndpoint(this IEndpointRouteBuilder group)
    {
        var app = group.MapGroup("users")
            .WithTags("Users")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET USER
        app.MapGet("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(new UserGetQuery(id), cancellationToken);
            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();

        // CREATE USER
        app.MapPost(string.Empty, async (
            UserCreateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(request, cancellationToken);
            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();

        //  UPDATE USER
        app.MapPut(string.Empty, async (
            UserUpdateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(request, cancellationToken);
            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();

        // DELETE USER
        app.MapDelete("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var res = await _sender.Send(new UserDeleteCommand(id), cancellationToken);
            return res.IsSuccessful ? Results.Ok
            (res) : Results.InternalServerError(res);
        })
            .Produces<Result<string>>();
    }
}
