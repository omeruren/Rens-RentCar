using GenericRepository;
using Rens_RentCar.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Vehicles;

[Permission("vehicle:delete")]
public sealed record VehicleDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class VehicleDeleteCommandHandler(IVehicleRepository _vehicleRepository, IUnitOfWork _unitOfWork) : IRequestHandler<VehicleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VehicleDeleteCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (vehicle is null)
            return Result<string>.Failure("Vehicle not found.");

        vehicle.Delete();
        _vehicleRepository.Update(vehicle);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Vehicle deleted successfully.";
    }
}
