using Rens_RentCar.Server.Application.Auth;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class AuthModule
{
    public static void MapAutEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/auth").WithTags("Auth");

        // LOGIN

        app.MapPost("/login", async (
            LoginCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);
        })
            .RequireRateLimiting("login-fixed")
            .Produces<Result<string>>();

        // FORGOT PASSWORD

        app.MapPost("/forgot-password/{email}", async (
            string email,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new ForgotPasswordCommand(email), cancellationToken);

            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .RequireRateLimiting("forgot-password-fixed")
            .Produces<Result<string>>();
    }
}
