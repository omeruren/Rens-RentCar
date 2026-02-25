using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Server.Application.Categories;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class CategoryModule
{
    public static void MapCategoryEndpoint(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/categories")
            .WithTags("Categories")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        // GET BY ID
        app.MapGet("{id}", async (
          Guid id,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(new CategoryGetQuery(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<CategoryDto>>();


        // POST
        app.MapPost(string.Empty, async (
            CategoryCreateCommand request,
            ISender _sender,
            CancellationToken cancellationToken) =>
        {
            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();

        // PUT
        app.MapPut(string.Empty, async (
            CategoryUpdateCommand request,
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
            var result = await _sender.Send(new CategoryDeleteCommand(id), cancellationToken);
            return result.IsSuccessful ? Results.Ok(result) : Results.InternalServerError(result);

        })
            .Produces<Result<string>>();
    }
}
