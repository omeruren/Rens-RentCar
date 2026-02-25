using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Categories;

[Permission("category:edit")]
public sealed record CategoryUpdateCommand(Guid Id, string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class CategoryUpdateCommandValidator : AbstractValidator<CategoryUpdateCommand>
{
    public CategoryUpdateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Category name is required.");
    }
}

internal sealed class CategoryUpdateCommandHandler(ICategoryRepository _categoryRepository, IUnitOfWork _unitOfWork) : IRequestHandler<CategoryUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return Result<string>.Failure("Category not found.");

        var isCategoryNameExists = await _categoryRepository.AnyAsync(c => c.Name.Value == request.Name && c.Id != request.Id, cancellationToken);

        if (isCategoryNameExists)
            return Result<string>.Failure("Category name is already taken before by someone else.");

        Name name = new(request.Name);
        category.SetName(name);
        category.SetStatus(request.IsActive);

        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Category updated successfully.";
    }
}
