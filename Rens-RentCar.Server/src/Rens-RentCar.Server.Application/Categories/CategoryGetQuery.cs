using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Categories;

[Permission("category:view")]
public sealed record CategoryGetQuery(Guid Id) : IRequest<Result<CategoryDto>>;

internal sealed class CategoryGetQueryHandler(ICategoryRepository _categoryRepository) : IRequestHandler<CategoryGetQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(CategoryGetQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
            return Result<CategoryDto>.Failure("Category not found.");

        return category;
    }
}
