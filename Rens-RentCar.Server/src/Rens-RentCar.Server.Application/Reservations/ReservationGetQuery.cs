using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Customers;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.Reservations;
using Rens_RentCar.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Reservations;
[Permission("reservations:view")]
public sealed record ReservationGetQuery(
    Guid Id) : IRequest<Result<ReservationDto>>;

internal sealed class ReservationGetQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository,
    IBranchRepository brancheRepository,
    IVehicleRepository vehicleRepository,
    ICategoryRepository categoryRepository,
    IProtectionPackageRepository protectionPackageRepository,
    IExtraRepository extraRepository
    ) : IRequestHandler<ReservationGetQuery, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(ReservationGetQuery request, CancellationToken cancellationToken)
    {
        var res = await reservationRepository.GetAllWithAuditInfos().MapTo(
            customerRepository.GetAll(),
            brancheRepository.GetAll(),
            vehicleRepository.GetAll(),
            categoryRepository.GetAll(),
            protectionPackageRepository.GetAll(),
            extraRepository.GetAll())
            .Where(i => i.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
        {
            return Result<ReservationDto>.Failure("Reservation not found.");
        }

        return res;
    }
}