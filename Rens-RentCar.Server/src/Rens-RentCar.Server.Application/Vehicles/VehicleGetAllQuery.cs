using Rens_RentCar.Domain.Vehicles;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetAllQuery : IRequest<IQueryable<VehicleDto>>;

internal sealed class VehicleGetAllQueryHandler(IVehicleRepository _vehicleRepository) : IRequestHandler<VehicleGetAllQuery, IQueryable<VehicleDto>>
{
    public Task<IQueryable<VehicleDto>> Handle(VehicleGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _vehicleRepository
            .GetAllWithAuditInfos()
            .MapTo()
            .AsQueryable();

        return Task.FromResult(res);
    }
}
