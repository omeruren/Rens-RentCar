using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rens_RentCar.Domain.Vehicles;

namespace Rens_RentCar.Server.Infrastructure.Configurations;

internal sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Brand);
        builder.OwnsOne(x => x.Model);
        builder.OwnsOne(x => x.ModelYear);
        builder.OwnsOne(x => x.Color);
        builder.OwnsOne(x => x.Plate);
        builder.OwnsOne(x => x.VinNumber);
        builder.OwnsOne(x => x.EngineNumber);
        builder.OwnsOne(x => x.Description);
        builder.OwnsOne(x => x.ImageUrl);
        builder.OwnsOne(x => x.FuelType);
        builder.OwnsOne(x => x.Transmission);
        builder.OwnsOne(x => x.EngineVolume);
        builder.OwnsOne(x => x.EnginePower);
        builder.OwnsOne(x => x.TractionType);
        builder.OwnsOne(x => x.FuelConsumption);
        builder.OwnsOne(x => x.SeatCount);
        builder.OwnsOne(x => x.Kilometer);
        builder.OwnsOne(x => x.DailyPrice);
        builder.OwnsOne(x => x.WeeklyDiscountRate);
        builder.OwnsOne(x => x.MonthlyDiscountRate);
        builder.OwnsOne(x => x.InsuranceType);
        builder.OwnsOne(x => x.LastMaintenanceDate);
        builder.OwnsOne(x => x.LastMaintenanceKm);
        builder.OwnsOne(x => x.NextMaintenanceKm);
        builder.OwnsOne(x => x.InspectionDate);
        builder.OwnsOne(x => x.InsuranceEndDate);
        builder.OwnsOne(x => x.CascoEndDate);
        builder.OwnsOne(x => x.TireStatus);
        builder.OwnsOne(x => x.GeneralStatus);
        builder.OwnsMany(x => x.Features);
    }
}
