namespace Rens_RentCar.Domain.Reservations.ValueObjects;

public sealed record ReservationExtra(
    Guid ExtraId,
    decimal Price);