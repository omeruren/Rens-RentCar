using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.Extras.ValueObjects;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Extras;

[Permission("extra:create")]
public sealed record ExtraCreateCommand(string Name, decimal Price, string Description, bool IsActive) : IRequest<Result<string>>;

public sealed class ExtraCreateCommandValidator : AbstractValidator<ExtraCreateCommand>
{
    public ExtraCreateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Extra name is required.");
        RuleFor(r => r.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal sealed class ExtraCreateCommandHandler(IExtraRepository _extraRepository, IUnitOfWork _unitOfWork) : IRequestHandler<ExtraCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExtraCreateCommand request, CancellationToken cancellationToken)
    {
        var exists = await _extraRepository.AnyAsync(e => e.Name.Value == request.Name, cancellationToken);

        if (exists)
            return Result<string>.Failure("Extra name is already taken before by someone else.");

        Name name = new(request.Name);
        Price price = new(request.Price);
        Description description = new(request.Description);

        Extra extra = new(name, price, description, request.IsActive);
        _extraRepository.Add(extra);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Extra created successfully.";
    }
}
