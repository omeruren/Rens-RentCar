using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.Extras.ValueObjects;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Extras;

[Permission("extra:edit")]
public sealed record ExtraUpdateCommand(Guid Id, string Name, decimal Price, string Description, bool IsActive) : IRequest<Result<string>>;

public sealed class ExtraUpdateCommandValidator : AbstractValidator<ExtraUpdateCommand>
{
    public ExtraUpdateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Extra name is required.");
        RuleFor(r => r.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal sealed class ExtraUpdateCommandHandler(IExtraRepository _extraRepository, IUnitOfWork _unitOfWork) : IRequestHandler<ExtraUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExtraUpdateCommand request, CancellationToken cancellationToken)
    {
        var extra = await _extraRepository.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (extra is null)
            return Result<string>.Failure("Extra not found.");

        var isNameExists = await _extraRepository.AnyAsync(e => e.Name.Value == request.Name && e.Id != request.Id, cancellationToken);

        if (isNameExists)
            return Result<string>.Failure("Extra name is already taken before by someone else.");

        Name name = new(request.Name);
        Price price = new(request.Price);
        Description description = new(request.Description);

        extra.SetName(name);
        extra.SetPrice(price);
        extra.SetDescription(description);
        extra.SetStatus(request.IsActive);

        _extraRepository.Update(extra);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Extra updated successfully.";
    }
}
