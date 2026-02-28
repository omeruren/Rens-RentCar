using Microsoft.AspNetCore.Mvc;
using Rens_RentCar.Domain.Vehicles;
using Rens_RentCar.Server.Application.Vehicles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class VehicleModule
{
    public static void MapVehicleEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/vehicles")
            .WithTags("Vehicles")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET BY ID
        app.MapGet("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new VehicleGetQuery(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<VehicleDto>>();

        // POST
        app.MapPost(string.Empty, async (
            [FromForm] VehicleCreateCommand request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);
        })
            .Accepts<VehicleCreateCommand>("multipart/form-data")
            .Produces<Result<string>>()
            .DisableAntiforgery();

        // PUT
        app.MapPut(string.Empty, async (
            [FromForm] VehicleUpdateCommand request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);
        })
            .Accepts<VehicleUpdateCommand>("multipart/form-data")
            .Produces<Result<string>>()
            .DisableAntiforgery();

        // DELETE
        app.MapDelete("{id}", async (
            Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new VehicleDeleteCommand(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();
    }
}
