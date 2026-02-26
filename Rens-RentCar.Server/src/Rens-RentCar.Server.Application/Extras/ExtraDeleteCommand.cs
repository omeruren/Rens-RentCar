using GenericRepository;
using Rens_RentCar.Domain.Extras;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Extras;

[Permission("extra:delete")]
public sealed record ExtraDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class ExtraDeleteCommandHandler(IExtraRepository _extraRepository, IUnitOfWork _unitOfWork) : IRequestHandler<ExtraDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExtraDeleteCommand request, CancellationToken cancellationToken)
    {
        var extra = await _extraRepository.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (extra is null)
            return Result<string>.Failure("Extra not found.");

        extra.Delete();
        _extraRepository.Update(extra);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Extra deleted successfully.";
    }
}
