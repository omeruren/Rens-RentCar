using Rens_RentCar.Server.Application.Auth;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class AuthModule
{
    public static void MapAutEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/auth").WithTags("Auth").RequireRateLimiting("login-fixed");

        app.MapPost("/login", async (
            LoginCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);
        })
            .Produces<Result<string>>();
    }
}
