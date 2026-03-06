using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Customers;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.Vehicles;

namespace Rens_RentCar.Domain.Reservations;



public sealed class ReservationExtraDto
{
    public Guid ExtraId { get; set; }
    public string ExtraName { get; set; } = default!;
    public decimal Price { get; set; }
}
public sealed class ReservationDto : BaseEntityDto
{
    public Guid CustomerId { get; set; } = default!;
    public string CustomerFullName { get; set; } = default!;
    public string CustomerNationalId { get; set; } = default!;
    public string CustomerPhoneNumber { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerFullAddress { get; set; } = default!;

    public Guid PickUpLocationId { get; set; } = default!;

    public string PickUpName { get; set; } = default!;
    public string PickUpFullAddress { get; set; } = default!;
    public string PickUpPhoneNumber { get; set; } = default!;

    public DateOnly PickUpDate { get; set; } = default!;
    public TimeOnly PickUpTime { get; set; } = default!;
    public DateTime PickUpDateTime { get; set; } = default!;
    public DateOnly DeliveryDate { get; set; } = default!;
    public TimeOnly DeliveryTime { get; set; } = default!;
    public DateTime DeliveryDateTime { get; set; } = default!;

    public Guid VehicleId { get; set; } = default!;
    public decimal VehicleDailyPrice { get; set; } = default!;
    public string VehicleBrand { get; set; } = default!;
    public string VehicleModel { get; set; } = default!;
    public int VehicleModelYear { get; set; } = default!;
    public string VehicleColor { get; set; } = default!;
    public string VehicleCategoryName { get; set; } = default!;
    public decimal VehicleFuelConsumption { get; set; } = default!;
    public int VehicleSeatCount { get; set; } = default!;
    public string VehicleTractionType { get; set; } = default!;
    public int VehicleKilometer { get; set; } = default!;
    public string VehicleImageUrl { get; set; } = default!;

    public Guid ProtectionPackageId { get; set; } = default!;
    public decimal ProtectionPackagePrice { get; set; } = default!;
    public string ProtectionPackageName { get; set; } = default!;
    public List<ReservationExtraDto> ReservationExtras { get; set; } = default!;
    public string Note { get; set; } = default!;
    public decimal Total { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int TotalDay { get; set; } = default!;
}

public static class ReservationExtensions
{
    public static IQueryable<ReservationDto> MapTo(
        this IQueryable<EntityWithAuditDto<Reservation>> entities,
             IQueryable<Customer> customers,
             IQueryable<Branch> branches,
             IQueryable<Vehicle> vehicles,
             IQueryable<Category> categories,
             IQueryable<ProtectionPackage> protectionPackages,
             IQueryable<Extra> extras
       )
    {
        var res = entities
            .Join(customers, m => m.Entity.CustomerId, m => m.Id, (r, customer) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                Customer = customer
            })
            .Join(branches, m => m.Entity.PickUpLocationId, m => m.Id, (r, branch) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                r.Customer,
                Branch = branch
            })
            .Join(protectionPackages, m => m.Entity.ProtectionPackageId, m => m.Id, (r, protectionPackage) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                r.Customer,
                r.Branch,
                ProtectionPackage = protectionPackage
            })
            .Join(vehicles, m => m.Entity.VehicleId, m => m.Id, (r, vehicle) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                r.Customer,
                r.Branch,
                r.ProtectionPackage,
                Vehicle = vehicle
            })
            .Select(s => new ReservationDto
            {
                Id = s.Entity.Id,

                CustomerId = s.Entity.CustomerId,
                CustomerEmail = s.Customer.Email.Value,
                CustomerFullAddress = s.Customer.FullAddress.Value,
                CustomerFullName = s.Customer.FullName.Value,
                CustomerNationalId = s.Customer.NationalId.Value,
                CustomerPhoneNumber = s.Customer.PhoneNumber.Value,

                PickUpLocationId = s.Entity.PickUpLocationId,
                PickUpName = s.Branch.Name.Value,
                PickUpFullAddress = s.Branch.Address.FullAddress,
                PickUpPhoneNumber = s.Branch.Contact.PhoneNumber1,

                PickUpDate = s.Entity.PickUpDate.Value,
                PickUpTime = s.Entity.PickUpTime.Value,
                PickUpDateTime = new DateTime(s.Entity.PickUpDate.Value, s.Entity.PickUpTime.Value),
                DeliveryDate = s.Entity.DeliveryDate.Value,
                DeliveryTime = s.Entity.DeliveryTime.Value,
                DeliveryDateTime = new DateTime(s.Entity.DeliveryDate.Value, s.Entity.DeliveryTime.Value),
                VehicleId = s.Entity.VehicleId,
                VehicleDailyPrice = s.Entity.VehicleDailyPrice.Value,

                VehicleBrand = s.Vehicle.Brand.Value,
                VehicleModel = s.Vehicle.Model.Value,
                VehicleModelYear = s.Vehicle.ModelYear.Value,
                VehicleCategoryName = categories.First(i => i.Id == s.Vehicle.CategoryId).Name.Value,
                VehicleColor = s.Vehicle.Color.Value,
                VehicleFuelConsumption = s.Vehicle.FuelConsumption.Value,
                VehicleSeatCount = s.Vehicle.SeatCount.Value,
                VehicleTractionType = s.Vehicle.TractionType.Value,
                VehicleKilometer = s.Vehicle.Kilometer.Value,
                VehicleImageUrl = s.Vehicle.ImageUrl.Value,

                ProtectionPackageId = s.Entity.ProtectionPackageId.Value,
                ProtectionPackagePrice = s.Entity.ProtectionPackagePrice.Value,
                ProtectionPackageName = s.ProtectionPackage.Name.Value,
                ReservationExtras = s.Entity.ReservationExtras.Join(extras, m => m.ExtraId, m => m.Id, (re, extra) => new ReservationExtraDto
                {
                    ExtraId = re.ExtraId,
                    ExtraName = extra.Name.Value,
                    Price = re.Price
                }).ToList(),
                Note = s.Entity.Note.Value,
                Total = s.Entity.Total.Value,
                TotalDay = s.Entity.TotalDay.Value,
                Status = s.Entity.Status.Value,
                IsActive = s.Entity.IsActive,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy.Value,
                CreatedFullName = s.CreatedUser.FullName.Value,
                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
                UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,
            });
        return res;
    }
}