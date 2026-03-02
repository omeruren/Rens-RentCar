using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.ProtectionPackages.ValueObjects;
using Rens_RentCar.Domain.Shared;
using Rens_RentCar.Domain.Vehicles;
using Rens_RentCar.Domain.Vehicles.ValueObjects;
using TS.Result;

namespace Rens_RentCar.Server.WebAPI.Modules;

public static class SeedModule
{
    public static void MapSeedData(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("seed-data").RequireAuthorization().WithTags("SeedData");

        //categories
        app.MapGet("categories",
            async (ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
            {
                var categoryNames = await categoryRepository.GetAll().Select(s => s.Name.Value).ToListAsync(cancellationToken);

                List<Category> newCategories = new()
                {
                    new(new Name("Sedan"), true),
                    new(new Name("Station Wagon"), true),
                    new(new Name("Minibus"), true),
                    new(new Name("SUV"), true),
                    new(new Name("Convertible"), true),
                    new(new Name("MPV"), true)
                };

                var list = newCategories.Where(p => !categoryNames.Contains(p.Name.Value)).ToList();
                categoryRepository.AddRange(list);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Category seed data completed successfully"));
            })
            .Produces<Result<string>>();

        //protection-packages
        app.MapGet("protection-packages",
            async (IProtectionPackageRepository repository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
            {
                var existingNames = await repository.GetAll().Select(p => p.Name.Value).ToListAsync(cancellationToken);

                var packages = new List<ProtectionPackage>
                {
                    new(
                        new("Mini Protection Package"),
                        new(150),
                        new(false),
                        true,
                        new List<ProtectionCoverage>
                        {
                            new("Collision Damage Waiver (CDW)"),
                            new("Theft Protection (TP)")
                        }),

                    new(
                        new("Standard Protection Package"),
                        new(250),
                        new(true),
                        true,
                        new List<ProtectionCoverage>
                        {
                            new("All Features of Previous Package Included"),
                            new("In Addition to Mini Package: Tire, Glass, Headlight Protection (TWH)"),
                            new("Minor Damage Protection (MI)")
                        }),

                    new(
                        new("Full Protection Package"),
                        new(350),
                        new(false),
                        true,
                        new List<ProtectionCoverage>
                        {
                            new("All Features of Previous Package Included"),
                            new("In Addition to Standard Package: Additional Driver"),
                            new("Young Driver")
                        }),
                };

                var newPackages = packages.Where(p => !existingNames.Contains(p.Name.Value)).ToList();
                repository.AddRange(newPackages);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Protection package seed data completed successfully"));
            })
            .Produces<Result<string>>();

        //extras
        app.MapGet("extras",
            async (IExtraRepository repository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
            {
                var existingNames = await repository.GetAll().Select(e => e.Name.Value).ToListAsync(cancellationToken);

                List<Extra> extras = new()
                {
                    // Recommended Protections
                    new(new("Minor Damage Protection"), new(114), new("Valid driver's license, acceptance of rental terms (no credit check, deposit excluded), and the additional driver must also be present at the office during vehicle delivery."), true),
                    new(new("Winter Tires"), new(246), new("Winter Tires (subject to availability)"), true),

                    // Additional Driver Package
                    new(new("Young Driver Package"), new(530), new("Allows you to rent a vehicle from the age group above yours."), true),
                    new(new("Debit Card Rental"), new(3193), new("Customers who meet the rental conditions with a valid debit card can purchase this product to continue renting a vehicle."), true),
                    new(new("No Deposit Rental"), new(1064), new("Customers who do not want to pay a deposit can rent a vehicle with this product. No deposit is required at the end of the contract, provided the vehicle is not stolen."), true),

                    // Seat Adapters
                    new(new("Booster Seat"), new(290), new("For children aged 4 and over (15-36 kg), rear seat booster seats must be used."), true),
                    new(new("Child Seat"), new(290), new("For children up to age 4 (9-18 kg), a child safety seat is mounted in the vehicle."), true),
                    new(new("Baby Seat"), new(290), new("For infants aged 0 (0 kg), an infant carrier model mounted on the rear seat."), true),
                };

                var newExtras = extras.Where(e => !existingNames.Contains(e.Name.Value)).ToList();
                repository.AddRange(newExtras);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Extra seed data completed successfully"));
            })
            .Produces<Result<string>>();

        //vehicles
        app.MapGet("vehicles",
            async (
                IVehicleRepository vehicleRepository,
                ICategoryRepository categoryRepository,
                IUnitOfWork unitOfWork,
                CancellationToken cancellationToken) =>
            {
                var branchId = Guid.Parse("0197de0b-7613-7846-af49-b5a2cc121576");
                var existingPlates = await vehicleRepository.GetAll().Select(v => v.Plate.Value).ToListAsync(cancellationToken);
                var categories = await categoryRepository.GetAll().ToListAsync(cancellationToken);

                var imageUrls = new[]
                {
                    "citroen-c3.jpg",
                    "fiat-egea-sedan.jpg",
                    "fiat-fiorino.jpg",
                    "renault-clio.jpg",
                    "hyundai-bayon.jpg",
                    "renault-megane-sedan.jpg",
                    "suzuki-vitara.jpg"
                };

                var brands = new[] { "Toyota", "Volkswagen", "Renault", "Ford", "Hyundai", "Peugeot", "Opel", "Honda", "BMW" };
                var models = new[] { "C3", "Egea", "Fiorino", "Clio", "Bayon", "Megane", "Vitara" };
                var allFeatures = new[]
                {
                    "Airbag", "ABS", "ESP", "Alarm System",
                    "GPS Navigation", "Parking Sensor", "Rear View Camera", "Cruise Control",
                    "Air Conditioning", "Heated Seats", "Sunroof", "Bluetooth",
                    "Touchscreen Display", "USB Port", "Premium Sound System", "Apple CarPlay"
                };
                var vehicles = new List<Vehicle>();
                var random = new Random();
                int imageIndex = 0;

                foreach (var category in categories)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var featureCount = new Random().Next(4, 9); // 4-8 features
                        var selectedFeatures = allFeatures.OrderBy(_ => new Random().Next()).Take(featureCount).ToList();
                        var features = selectedFeatures.Select(f => new Feature(f)).ToList();
                        var brand = brands[random.Next(brands.Length)];
                        var model = models[random.Next(models.Length)];
                        var plate = $"34{brand[..2].ToUpper()}{category.Name.Value[..1].ToUpper()}{i + 1}";

                        if (existingPlates.Contains(plate))
                            continue;

                        var vehicle = new Vehicle(
                            new Brand(brand),
                            new Model($"{model} {category.Name.Value[..2].ToUpper()}"),
                            new ModelYear(2022 + i % 2),
                            new Color("Gray"),
                            new Plate(plate),
                            new IdentityId(category.Id),
                            new IdentityId(branchId),
                            new VinNumber($"VIN{category.Name.Value[..1].ToUpper()}{i}{Guid.NewGuid():N}"[..16]),
                            new EngineNumber($"ENG{i}{Guid.NewGuid():N}"[..12]),
                            new Description($"{brand} {model} description"),
                            new ImageUrl(imageUrls[imageIndex++ % imageUrls.Length]),
                            new FuelType(i % 2 == 0 ? "Gasoline" : "Diesel"),
                            new Transmission(i % 2 == 0 ? "Automatic" : "Manual"),
                            new EngineVolume(1.4m + i * 0.1m),
                            new EnginePower(100 + i * 10),
                            new TractionType("Front-Wheel Drive"),
                            new FuelConsumption(5.5m + i * 0.3m),
                            new SeatCount(5),
                            new Kilometer(10000 + i * 3000),
                            new DailyPrice(1800 + i * 50),
                            new WeeklyDiscountRate(10),
                            new MonthlyDiscountRate(15),
                            new InsuranceType("Casco & Insurance"),
                            new LastMaintenanceDate(DateOnly.FromDateTime(DateTime.Now.AddMonths(-3))),
                            new LastMaintenanceKm(15000 + i * 1000),
                            new NextMaintenanceKm(20000 + i * 1000),
                            new InspectionDate(DateOnly.FromDateTime(DateTime.Now.AddMonths(9))),
                            new InsuranceEndDate(DateOnly.FromDateTime(DateTime.Now.AddYears(1))),
                            new CascoEndDate(DateOnly.FromDateTime(DateTime.Now.AddYears(1))),
                            new TireStatus("Good"),
                            new GeneralStatus("Very Good"),
                            true,
                            features
                        );

                        vehicles.Add(vehicle);
                    }
                }

                vehicleRepository.AddRange(vehicles);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("All vehicles added successfully"));
            })
            .Produces<Result<string>>();

    }
}
