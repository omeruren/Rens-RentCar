using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetQuery(Guid Id) : IRequest<Result<VehicleDto>>;

internal sealed class VehicleGetQueryHandler(
    IVehicleRepository _vehicleRepository,
    IBranchRepository branchRepository,
    ICategoryRepository categoryRepository
    ) : IRequestHandler<VehicleGetQuery, Result<VehicleDto>>
{
    public async Task<Result<VehicleDto>> Handle(VehicleGetQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository
            .GetAllWithAuditInfos()
            .MapTo(branchRepository.GetAll(), categoryRepository.GetAll())
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (vehicle is null)
            return Result<VehicleDto>.Failure("Vehicle not found.");

        return vehicle;
    }
}
