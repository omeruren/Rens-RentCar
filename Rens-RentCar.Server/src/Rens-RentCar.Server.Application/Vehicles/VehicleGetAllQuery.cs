using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Vehicles;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetAllQuery : IRequest<IQueryable<VehicleDto>>;

internal sealed class VehicleGetAllQueryHandler(
    IVehicleRepository _vehicleRepository, ICategoryRepository categoryRepository, IBranchRepository branchRepository
    ) : IRequestHandler<VehicleGetAllQuery, IQueryable<VehicleDto>>
{
    public Task<IQueryable<VehicleDto>> Handle(VehicleGetAllQuery request, CancellationToken cancellationToken)
    {
        var res = _vehicleRepository
            .GetAllWithAuditInfos()
         .MapTo(branchRepository.GetAll(), categoryRepository.GetAll())
            .AsQueryable();

        return Task.FromResult(res);
    }
}
