using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.ProtectionPackages.ValueObjects;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.ProtectionPackages;

[Permission("protection:edit")]
public sealed record ProtectionUpdateCommand(Guid Id, string Name, decimal Price, bool IsRecommended, IEnumerable<string> Coverages) : IRequest<Result<string>>;

public sealed class ProtectionUpdateCommandValidator : AbstractValidator<ProtectionUpdateCommand>
{
    public ProtectionUpdateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Protection package name is required.");
        RuleFor(r => r.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal sealed class ProtectionUpdateCommandHandler(IProtectionPackageRepository _protectionRepository, IUnitOfWork _unitOfWork) : IRequestHandler<ProtectionUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionUpdateCommand request, CancellationToken cancellationToken)
    {
        var protection = await _protectionRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (protection is null)
            return Result<string>.Failure("Protection package not found.");

        var isNameExists = await _protectionRepository.AnyAsync(p => p.Name.Value == request.Name && p.Id != request.Id, cancellationToken);

        if (isNameExists)
            return Result<string>.Failure("Protection package name is already taken before by someone else.");

        Name name = new(request.Name);
        Price price = new(request.Price);
        IsRecommended isRecommended = new(request.IsRecommended);
        var coverages = request.Coverages.Select(c => new ProtectionCoverage(c));

        protection.SetName(name);
        protection.SetPrice(price);
        protection.SetIsRecommended(isRecommended);
        protection.SetCoverages(coverages);

        _protectionRepository.Update(protection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Protection package updated successfully.";
    }
}
