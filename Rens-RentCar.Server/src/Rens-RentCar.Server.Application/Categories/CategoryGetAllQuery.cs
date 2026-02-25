using Rens_RentCar.Domain.Categories;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Categories;

[Permission("category:view")]
public sealed record CategoryGetAllQuery : IRequest<IQueryable<CategoryDto>>;

internal sealed class CategoryGetAllQueryHandler(ICategoryRepository _categoryRepository) : IRequestHandler<CategoryGetAllQuery, IQueryable<CategoryDto>>
{
    public Task<IQueryable<CategoryDto>> Handle(CategoryGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _categoryRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .AsQueryable();

        return Task.FromResult(res);
    }
}
