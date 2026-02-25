using GenericRepository;
using Rens_RentCar.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Categories;

[Permission("category:delete")]
public sealed record CategoryDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class CategoryDeleteCommandHandler(ICategoryRepository _categoryRepository, IUnitOfWork _unitOfWork) : IRequestHandler<CategoryDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return Result<string>.Failure("Category not found.");

        category.Delete();
        _categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Category deleted successfully.";
    }
}
