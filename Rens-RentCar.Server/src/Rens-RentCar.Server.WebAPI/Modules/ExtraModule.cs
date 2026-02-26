using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Server.Application.Extras;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class ExtraModule
{
    public static void MapExtraEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/extras")
            .WithTags("Extras")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET BY ID
        app.MapGet("{id}", async (
          Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new ExtraGetQuery(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<ExtraDto>>();


        // POST
        app.MapPost(string.Empty, async (
            ExtraCreateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();

        // PUT
        app.MapPut(string.Empty, async (
            ExtraUpdateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();

        // DELETE
        app.MapDelete("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new ExtraDeleteCommand(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();
    }
}
