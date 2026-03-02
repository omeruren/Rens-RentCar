using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Customers.ValueObjects;
using Rens_RentCar.Domain.Shared;

namespace Rens_RentCar.Domain.Customers;

public sealed class Customer : BaseEntity
{
    public NationalId NationalId { get; set; } = default!;
    public FirstName FirstName { get; set; } = default!;
    public LastName LastName { get; set; } = default!;
    public FullName FullName { get; set; } = default!;
    public BirthDate BirthDate { get; set; } = default!;
    public PhoneNumber PhoneNumber { get; set; } = default!;
    public Email Email { get; set; } = default!;
    public DrivingLicenseIssueDate DrivingLicenseIssueDate { get; set; } = default!;
    public FullAddress FullAddress { get; set; } = default!;


    public void SetNationalId(NationalId nationalId) => NationalId = nationalId;
    public void SetFirstName(FirstName firstName) => FirstName = firstName;
    public void SetLastName(LastName lastName) => LastName = lastName;
    public void SetBirthDate(BirthDate birthDate) => BirthDate = birthDate;
    public void SetFullName() => FullName = new(string.Join(" ", FirstName.Value, LastName.Value));
    public void SetPhoneNumber(PhoneNumber phoneNumber) => PhoneNumber = phoneNumber;
    public void SetEmail(Email email) => Email = email;
    public void SetDrivingLicenceIssuanceDate(DrivingLicenseIssueDate drivingLicenseIssuanceDate) => DrivingLicenseIssueDate = drivingLicenseIssuanceDate;
    public void SetFullAddress(FullAddress fullAddress) => FullAddress = fullAddress;
}
