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

[Permission("reservation:create")]

public sealed record CreditCartInformation(
    string CartNumber,
    string Owner,
    string Expiry,
    string CCV);

public sealed record ReservationCreateCommand(
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
   CreditCartInformation CreditCartInformation,
   decimal Total,
   List<ReservationExtra> ReservationExtras
) : IRequest<Result<string>>;

public sealed class ReservationCreateCommandValidator : AbstractValidator<ReservationCreateCommand>
{
    public ReservationCreateCommandValidator()
    {
        RuleFor(x => x.CreditCartInformation.CartNumber)
            .NotEmpty()
            .WithMessage("Card number is required.");

        RuleFor(x => x.CreditCartInformation.Owner)
            .NotEmpty()
            .WithMessage("Card Owner name is required.");

        RuleFor(x => x.CreditCartInformation.Expiry)
           .NotEmpty()
           .WithMessage("Expire date is required.");

        RuleFor(x => x.CreditCartInformation.CCV)
           .NotEmpty()
           .WithMessage("CCV is required.");

        RuleFor(x => x.PickUpDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Pick up date can not be early from today..");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Delivery time can not be early from today.");
    }
}


internal sealed class ReservationCreateCommandHandler(
    IBranchRepository _branchRepository,
    ICustomerRepository _customerRepository,
    IReservationRepository _reservationRepository,
    IVehicleRepository _vehicleRepository,
    IClaimContext _claimContext,
    IUnitOfWork _unitOfWork) : IRequestHandler<ReservationCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ReservationCreateCommand request, CancellationToken cancellationToken)
    {
        var locationId = request.PickUpLocationId ?? _claimContext.GetBranchId();

        #region Branch, Customer and Vehicle controls
        var isBranchExists = await _branchRepository.AnyAsync(i => i.Id == locationId, cancellationToken);
        if (!isBranchExists)
        {
            return Result<string>.Failure("Branch not found.");
        }

        var isCustomerExists = await _customerRepository.AnyAsync(i => i.Id == request.CustomerId, cancellationToken);
        if (!isCustomerExists)
        {
            return Result<string>.Failure("Customer not found.");
        }

        var isVehicleExists = await _vehicleRepository.AnyAsync(i => i.Id == request.VehicleId, cancellationToken);
        if (!isVehicleExists)
        {
            return Result<string>.Failure("Vehicle not found.");
        }
        #endregion

        #region Vehicle availability control  

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
        #endregion

        #region Payment Process
        // if payment is successful, continue
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
        IEnumerable<ReservationExtra> reservationExtras = request.ReservationExtras.Select(s => new ReservationExtra(s.ExtraId, s.Price));
        Note note = new(request.Note);
        var last4Digits = request.CreditCartInformation.CartNumber[^4..];
        Status status = Status.Pending;
        Total total = new(request.Total);
        PaymentInformation paymentInformation = new(last4Digits, request.CreditCartInformation.Owner);

        Reservation reservation = Reservation.Create(
            customerId,
            pickUpLocationId,
            pickUpDate,
            pickUpTime,
            deliveryDate,
            deliveryTime,
            note,
            paymentInformation,
            status,

            vehicleId,
            vehicleDailyPrice,
            protectionPackageId,
            protectionPackagePrice,
           total,
           reservationExtras
        );
        #endregion

        _reservationRepository.Add(reservation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reservation created successfully.";
    }
}