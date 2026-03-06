using Rens_RentCar.Domain.Reservations;
using Rens_RentCar.Server.Application.Reservations;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class ReservationModule
{
    public static void MapReservationEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/reservations")
            .WithTags("Reservations")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET BY ID
        app.MapGet("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new ReservationGetQuery(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<ReservationDto>>();

        // POST
        app.MapPost(string.Empty, async (
            ReservationCreateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();

        // PUT
        app.MapPut(string.Empty, async (
            ReservationUpdateCommand request,
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
            var result = await _sender.Send(new ReservationDeleteCommand(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();
    }
}
