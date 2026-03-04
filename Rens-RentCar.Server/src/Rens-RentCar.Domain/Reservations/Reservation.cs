using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Reservations.ValueObjects;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Reservations;

public sealed class Reservation : BaseEntity, IAggregate
{
    private readonly List<ReservationExtra> _reservationExtras = new();
    private Reservation() { }
    private Reservation(
        IdentityId customerId,

        IdentityId pickUpLocationId,
        PickUpDate pickUpDate,
        PickUpTime pickUpTime,

        DeliveryDate deliveryUpDate,
        DeliveryTime deliveryUpTime,

        Note note,
        PaymentInformation paymetnInformation,
        Status status,

        IdentityId vehicleId,
        Price vehicleDailyPrice,

        IdentityId protectionPackageId,
        Price protectionPackagePrice,


        Total total,
        IEnumerable<ReservationExtra> reservationExtras)
    {
        SetCustomerId(customerId);
        SetPickUpLocationId(pickUpLocationId);
        SetPickUpDate(pickUpDate);
        SetPickUpTime(pickUpTime);
        SetDeliveryDate(deliveryUpDate);
        SetDeliveryTime(deliveryUpTime);

        SetTotalDay();
        SetNote(note);

        SetPaymetnInformation(paymetnInformation);
        SetStatus(status);

        SetVehicleId(vehicleId);
        SetVehicleDailyPrice(vehicleDailyPrice);

        SetProtectionPackageId(protectionPackageId);
        SetProtectionPackagePrice(protectionPackagePrice);

        SetTotal(total);
        SetReservationExtras(reservationExtras);
    }

    public IdentityId CustomerId { get; private set; } = default!;
    public IdentityId PickUpLocationId { get; private set; } = default!;
    public PickUpDate PickUpDate { get; private set; } = default!;
    public PickUpTime PickUpTime { get; private set; } = default!;
    public DeliveryDate DeliveryDate { get; private set; } = default!;
    public DeliveryTime DeliveryTime { get; private set; } = default!;
    public TotalDay TotalDay { get; private set; } = default!;
    public Note Note { get; private set; } = default!;
    public PaymentInformation PaymentInformation { get; private set; } = default!;
    public Status Status { get; private set; } = default!;


    public IdentityId VehicleId { get; private set; } = default!;
    public Price VehicleDailyPrice { get; private set; } = default!;

    public IdentityId ProtectionPackageId { get; private set; } = default!;
    public Price ProtectionPackagePrice { get; private set; } = default!;

    public Total Total { get; private set; } = default!;
    public IReadOnlyCollection<ReservationExtra> ReservationExtras => _reservationExtras;

    public static Reservation Create(
      IdentityId customerId,
      IdentityId pickUpLocationId,
      PickUpDate pickUpDate,
      PickUpTime pickUpTime,
      DeliveryDate deliveryDate,
      DeliveryTime deliveryTime,
      Note note,
      PaymentInformation paymentInformation,
      Status status,
      IdentityId vehicleId,
      Price vehicleDailyPrice,
      IdentityId protectionPackageId,
      Price protectionPackagePrice,
      Total total,
      IEnumerable<ReservationExtra> reservationExtras

        )
    {
        var reservation = new Reservation(
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

        return reservation;
    }
    public void SetCustomerId(IdentityId customerId) => CustomerId = customerId;
    public void SetPickUpLocationId(IdentityId pickUpLocationId) => PickUpLocationId = pickUpLocationId;
    public void SetPickUpDate(PickUpDate pickUpDate) => PickUpDate = pickUpDate;
    public void SetPickUpTime(PickUpTime pickUpTime) => PickUpTime = pickUpTime;
    public void SetDeliveryDate(DeliveryDate deliveryDate) => DeliveryDate = deliveryDate;
    public void SetDeliveryTime(DeliveryTime deliveryTime) => DeliveryTime = deliveryTime;
    public void SetTotalDay()
    {
        var pickUpDateTime = PickUpDate.Value.ToDateTime(PickUpTime.Value);
        var deliveryDateTime = DeliveryDate.Value.ToDateTime(DeliveryTime.Value);

        var totalDays = (deliveryDateTime.Date - pickUpDateTime.Date).Days;

        var sameDayExtraAllowed = DeliveryTime.Value <= PickUpTime.Value.Add(TimeSpan.FromHours(2));

        if (totalDays == 0 || (totalDays == 1 && sameDayExtraAllowed))
        {
            TotalDay = new TotalDay(1);
        }
        else if (sameDayExtraAllowed)
        {
            TotalDay = new TotalDay(totalDays);
        }
        else
        {
            TotalDay = new TotalDay(totalDays + 1);
        }
    }

    public void SetNote(Note note) => Note = note;
    public void SetPaymetnInformation(PaymentInformation paymetnInformation) => PaymentInformation = paymetnInformation;
    public void SetStatus(Status status) => Status = status;
    public void SetVehicleId(IdentityId vehicleId) => VehicleId = vehicleId;
    public void SetVehicleDailyPrice(Price vehicleDailyPrice) => VehicleDailyPrice = vehicleDailyPrice;
    public void SetProtectionPackageId(IdentityId protectionPackageId) => ProtectionPackageId = protectionPackageId;
    public void SetProtectionPackagePrice(Price protectionPackagePrice) => ProtectionPackagePrice = protectionPackagePrice;
    public void SetReservationExtras(IEnumerable<ReservationExtra> reservationExtras)
    {
        _reservationExtras.Clear();
        _reservationExtras.AddRange(reservationExtras);

    }
    public void SetTotal(Total total)
    {
        Total = total;
    }


}

public sealed record Status(string Value)
{
    public static Status Pending => new("Pending");
    public static Status Completed => new("Completed");
    public static Status Delivered => new("Delivered");
    public static Status Cancelled => new("Cancelled");

}