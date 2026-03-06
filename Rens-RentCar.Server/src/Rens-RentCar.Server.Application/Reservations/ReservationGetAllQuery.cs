using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Customers;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.Reservations;
using Rens_RentCar.Domain.Vehicles;
using TS.MediatR;

namespace Rens_RentCar.Server.Application.Reservations;

[Permission("reservations:view")]
public sealed record ReservationGetAllQuery : IRequest<IQueryable<ReservationDto>>;

internal sealed class ReservationGetAllQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository,
    IBranchRepository brancheRepository,
    IVehicleRepository vehicleRepository,
    ICategoryRepository categoryRepository,
    IProtectionPackageRepository protectionPackageRepository,
    IExtraRepository extraRepository
    ) : IRequestHandler<ReservationGetAllQuery, IQueryable<ReservationDto>>
{
    public Task<IQueryable<ReservationDto>> Handle(ReservationGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            reservationRepository.GetAllWithAuditInfos()
            .MapTo(
                customerRepository.GetAll(),
                brancheRepository.GetAll(),
                vehicleRepository.GetAll(),
                categoryRepository.GetAll(),
                protectionPackageRepository.GetAll(),
                extraRepository.GetAll())
            .AsQueryable());
}