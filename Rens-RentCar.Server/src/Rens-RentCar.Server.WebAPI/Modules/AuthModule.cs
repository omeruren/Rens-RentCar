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

        // CHECK FORGOT PASSWORD CODE

        app.MapGet("/check-forgot-password-code/{forgotPasswordCode}", async (
            Guid forgotPasswordCode,

            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new CheckForgotPasswordCodeCommand(forgotPasswordCode), cancellationToken);

            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .RequireRateLimiting("check-forgot-password-code-fixed")
            .Produces<Result<string>>();

        // RESET PASSWORD

        app.MapPost("/reset-password", async (
            ResetPasswordCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .RequireRateLimiting("reset-password-fixed")
            .Produces<Result<string>>();
    }
}
