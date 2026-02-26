using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.ProtectionPackages.ValueObjects;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.ProtectionPackages;

[Permission("protection:create")]
public sealed record ProtectionCreateCommand(
    string Name,
    decimal Price,
    bool IsRecommended,
    IEnumerable<string> Coverages,
    bool IsActive) : IRequest<Result<string>>;

public sealed class ProtectionCreateCommandValidator : AbstractValidator<ProtectionCreateCommand>
{
    public ProtectionCreateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Protection package name is required.");
        RuleFor(r => r.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal sealed class ProtectionCreateCommandHandler(IProtectionPackageRepository _protectionRepository, IUnitOfWork _unitOfWork) : IRequestHandler<ProtectionCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionCreateCommand request, CancellationToken cancellationToken)
    {
        var isExists = await _protectionRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);

        if (isExists)
            return Result<string>.Failure("Protection package name is already taken.");

        Name name = new(request.Name);
        Price price = new(request.Price);
        IsRecommended isRecommended = new(request.IsRecommended);
        var coverages = request.Coverages.Select(c => new ProtectionCoverage(c));

        var protection = new ProtectionPackage(name, price, isRecommended, coverages);

        _protectionRepository.Add(protection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Protection package created successfully.";
    }
}
