using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Categories;

[Permission("category:create")]
public sealed record CategoryCreateCommand(string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Category name is required.");
    }
}

internal sealed class CategoryCreateCommandHandler(ICategoryRepository _categoryRepository, IUnitOfWork _unitOfWork) : IRequestHandler<CategoryCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var isCategoryExists = await _categoryRepository.AnyAsync(c => c.Name.Value == request.Name, cancellationToken);

        if (isCategoryExists)
            return Result<string>.Failure("Category name is already taken before by someone else.");

        Name name = new(request.Name);
        Category category = new(name, request.IsActive);
        _categoryRepository.Add(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return "Category Created successfully.";
    }
}
