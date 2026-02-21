namespace Rens_RentCar.Domain.Shared;

public sealed record Contact(
    string PhoneNumber1,
    string? PhoneNumber2,
    string? Email);