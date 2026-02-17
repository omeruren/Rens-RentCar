using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Server.Application.Branches;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class BranchModule
{
    public static void MapBranchEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/branches")
            .WithTags("Branches")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET BY ID
        app.MapGet("{id}", async (
          Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new BranchGetQuery(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<Branch>>();

        // POST
        app.MapPost(string.Empty, async (
            BranchCreateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();

        // PUT
        app.MapPut(string.Empty, async (
            BranchUpdateCommand request,
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
            var result = await _sender.Send(new BranchDeleteCommand(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();
    }
}
