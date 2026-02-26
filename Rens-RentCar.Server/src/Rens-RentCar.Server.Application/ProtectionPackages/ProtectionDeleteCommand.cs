using GenericRepository;
using Rens_RentCar.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.ProtectionPackages;

[Permission("protection:delete")]
public sealed record ProtectionDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class ProtectionDeleteCommandHandler(IProtectionPackageRepository _protectionRepository, IUnitOfWork _unitOfWork) : IRequestHandler<ProtectionDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionDeleteCommand request, CancellationToken cancellationToken)
    {
        var protection = await _protectionRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (protection is null)
            return Result<string>.Failure("Protection package not found.");

        protection.Delete();
        _protectionRepository.Update(protection);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Protection package deleted successfully.";
    }
}
