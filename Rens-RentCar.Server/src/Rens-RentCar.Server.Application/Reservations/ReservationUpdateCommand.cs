using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Customers;
using Rens_RentCar.Domain.Reservations;
using Rens_RentCar.Domain.Reservations.ValueObjects;
using Rens_RentCar.Domain.Shared;
using Rens_RentCar.Domain.Vehicles;
using Rens_RentCar.Server.Application.Services;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Reservations;

[Permission("reservation:edit")]
public sealed record ReservationUpdateCommand(
    Guid Id,
    Guid CustomerId,
    Guid? PickUpLocationId,
    DateOnly PickUpDate,
    TimeOnly PickUpTime,
    DateOnly DeliveryDate,
    TimeOnly DeliveryTime,
    Guid VehicleId,
    decimal VehicleDailyPrice,
    Guid ProtectionPackageId,
    decimal ProtectionPackagePrice,
    string Note,
    decimal Total,
    List<ReservationExtra> ReservationExtras
) : IRequest<Result<string>>;
public sealed class ReservationUpdateCommandValidator : AbstractValidator<ReservationUpdateCommand>
{
    public ReservationUpdateCommandValidator()
    {

        RuleFor(x => x.PickUpDate)
             .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
             .WithMessage("Pick up date can not be early from today.");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Delivery date can not be early from today.");

    }
}

internal sealed class ReservationUpdateCommandHandler(
    IBranchRepository _branchRepository,
    ICustomerRepository _customerRepository,
    IReservationRepository _reservationRepository,
    IVehicleRepository _vehicleRepository,
    IClaimContext _claimContext,
    IUnitOfWork _unitOfWork) : IRequestHandler<ReservationUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ReservationUpdateCommand request, CancellationToken cancellationToken)
    {
        Reservation? reservation = await _reservationRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (reservation is null)
        {
            return Result<string>.Failure("Reservation not found.");
        }

        if (reservation.Status == Status.Completed || reservation.Status == Status.Cancelled)
        {
            return Result<string>.Failure("This Reservation can not update now.(Reservation is completed or cancelled.)");
        }

        var locationId = request.PickUpLocationId ?? _claimContext.GetBranchId();

        #region Branch, Customer and Vehicle controls
        if (reservation.PickUpLocationId.Value != locationId)
        {
            var isBranchExists = await _branchRepository.AnyAsync(i => i.Id == locationId, cancellationToken);
            if (!isBranchExists)
            {
                return Result<string>.Failure("Branch not found.");
            }
        }

        if (reservation.CustomerId != request.CustomerId)
        {
            var isCustomerExists = await _customerRepository.AnyAsync(i => i.Id == request.CustomerId, cancellationToken);
            if (!isCustomerExists)
            {
                return Result<string>.Failure("Customer not found.");
            }
        }

        if (reservation.VehicleId != request.VehicleId)
        {
            var isVehicleExists = await _vehicleRepository.AnyAsync(i => i.Id == request.VehicleId, cancellationToken);
            if (!isVehicleExists)
            {
                return Result<string>.Failure("Vehicle not found.");
            }
        }
        #endregion

        #region Vehicle availability control
        if (reservation.PickUpDate.Value != request.PickUpDate
            || reservation.PickUpTime.Value != request.PickUpTime
            | reservation.DeliveryDate.Value != request.DeliveryDate
            || reservation.DeliveryTime.Value != request.DeliveryTime
            )
        {

            var requestedPickUp = request.PickUpDate.ToDateTime(request.PickUpTime);
            var requestedDelivery = request.DeliveryDate.ToDateTime(request.DeliveryTime);



            var possibleOverlaps = await _reservationRepository
                 .Where(r => r.VehicleId == request.VehicleId && (
                 r.Status.Value == Status.Pending.Value || r.Status.Value == Status.Delivered.Value))
                 .Select(s => new
                 {
                     Id = s.Id,
                     VehicleId = s.VehicleId,
                     DeliveryDate = s.DeliveryDate.Value,
                     DeliveryTime = s.DeliveryTime.Value,
                     PickUpDate = s.PickUpDate.Value,
                     PickUpTime = s.PickUpTime.Value,
                 })
                 .ToListAsync(cancellationToken);

            var overlaps = possibleOverlaps.Any(r =>
                requestedPickUp < r.DeliveryDate.ToDateTime(r.DeliveryTime).AddHours(1) &&
                requestedDelivery > r.PickUpDate.ToDateTime(r.PickUpTime)
            );

            if (overlaps)
            {
                return Result<string>.Failure("This vehicle is not available for the selected date and time range.");
            }
        }
        #endregion

        #region Reservation Object Creation
        IdentityId customerId = new(request.CustomerId);
        IdentityId pickUpLocationId = new(locationId);
        PickUpDate pickUpDate = new(request.PickUpDate);
        PickUpTime pickUpTime = new(request.PickUpTime);
        DeliveryDate deliveryDate = new(request.DeliveryDate);
        DeliveryTime deliveryTime = new(request.DeliveryTime);
        IdentityId vehicleId = new(request.VehicleId);
        Price vehicleDailyPrice = new(request.VehicleDailyPrice);
        IdentityId protectionPackageId = new(request.ProtectionPackageId);
        Price protectionPackagePrice = new(request.ProtectionPackagePrice);
        Note note = new(request.Note);
        Total total = new(request.Total);
        IEnumerable<ReservationExtra> reservationExtras = request.ReservationExtras.Select(s => new ReservationExtra(s.ExtraId, s.Price));

        reservation.SetCustomerId(customerId);
        reservation.SetPickUpLocationId(pickUpLocationId);
        reservation.SetPickUpDate(pickUpDate);
        reservation.SetPickUpTime(pickUpTime);
        reservation.SetDeliveryDate(deliveryDate);
        reservation.SetDeliveryTime(deliveryTime);
        reservation.SetVehicleId(vehicleId);
        reservation.SetVehicleDailyPrice(vehicleDailyPrice);
        reservation.SetProtectionPackageId(protectionPackageId);
        reservation.SetProtectionPackagePrice(protectionPackagePrice);
        reservation.SetTotal(total);
        reservation.SetReservationExtras(reservationExtras);
        reservation.SetNote(note);
        #endregion

        _reservationRepository.Update(reservation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reservation updated successfully.";
    }
}
