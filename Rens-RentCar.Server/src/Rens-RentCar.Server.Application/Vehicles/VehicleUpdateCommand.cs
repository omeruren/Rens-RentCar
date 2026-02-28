using FluentValidation;
using GenericFileService.Files;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Vehicles;
using Rens_RentCar.Domain.Vehicles.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Vehicles;

[Permission("vehicle:edit")]
public sealed record VehicleUpdateCommand(
    Guid Id,
    string Brand,
    string Model,
    int ModelYear,
    string Color,
    string Plate,
    Guid CategoryId,
    Guid BranchId,
    string VinNumber,
    string EngineNumber,
    string Description,
    IFormFile? File,
    string FuelType,
    string Transmission,
    decimal EngineVolume,
    int EnginePower,
    string TractionType,
    decimal FuelConsumption,
    int SeatCount,
    int Kilometer,
    decimal DailyPrice,
    decimal WeeklyDiscountRate,
    decimal MonthlyDiscountRate,
    string InsuranceType,
    DateTimeOffset LastMaintenanceDate,
    int LastMaintenanceKm,
    int NextMaintenanceKm,
    DateTimeOffset InspectionDate,
    DateTimeOffset InsuranceEndDate,
    DateTimeOffset CascoEndDate,
    string TireStatus,
    string GeneralStatus,
    bool IsActive,
    IEnumerable<string> Features) : IRequest<Result<string>>;

public sealed class VehicleUpdateCommandValidator : AbstractValidator<VehicleUpdateCommand>
{
    public VehicleUpdateCommandValidator()
    {
        RuleFor(r => r.Brand).NotEmpty().WithMessage("Brand is required.");
        RuleFor(r => r.Model).NotEmpty().WithMessage("Model is required.");
        RuleFor(r => r.ModelYear).GreaterThan(0).WithMessage("Model year must be greater than 0.");
        RuleFor(r => r.Color).NotEmpty().WithMessage("Color is required.");
        RuleFor(r => r.Plate).NotEmpty().WithMessage("Plate is required.");
        RuleFor(r => r.CategoryId).NotEmpty().WithMessage("Category is required.");
        RuleFor(r => r.BranchId).NotEmpty().WithMessage("Branch is required.");
        RuleFor(r => r.VinNumber).NotEmpty().WithMessage("VIN number is required.");
        RuleFor(r => r.EngineNumber).NotEmpty().WithMessage("Engine number is required.");
        RuleFor(r => r.DailyPrice).GreaterThan(0).WithMessage("Daily price must be greater than 0.");
        RuleFor(r => r.SeatCount).GreaterThan(0).WithMessage("Seat count must be greater than 0.");
    }
}

internal sealed class VehicleUpdateCommandHandler(IVehicleRepository _vehicleRepository, IUnitOfWork _unitOfWork) : IRequestHandler<VehicleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VehicleUpdateCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (vehicle is null)
            return Result<string>.Failure("Vehicle not found.");

        var isPlateExists = await _vehicleRepository.AnyAsync(v => v.Plate.Value == request.Plate && v.Id != request.Id, cancellationToken);

        if (isPlateExists)
            return Result<string>.Failure("A vehicle with this plate already exists.");

        string imageUrl = vehicle.ImageUrl.Value;
        if (request.File is not null && request.File.Length > 0)
            imageUrl = FileService.FileSaveToServer(request.File, "wwwroot/images/");

        vehicle.SetBrand(new Brand(request.Brand));
        vehicle.SetModel(new Model(request.Model));
        vehicle.SetModelYear(new ModelYear(request.ModelYear));
        vehicle.SetColor(new Color(request.Color));
        vehicle.SetPlate(new Plate(request.Plate));
        vehicle.SetCategoryId(new IdentityId(request.CategoryId));
        vehicle.SetBranchId(new IdentityId(request.BranchId));
        vehicle.SetVinNumber(new VinNumber(request.VinNumber));
        vehicle.SetEngineNumber(new EngineNumber(request.EngineNumber));
        vehicle.SetDescription(new Description(request.Description));
        vehicle.SetImageUrl(new ImageUrl(imageUrl));
        vehicle.SetFuelType(new FuelType(request.FuelType));
        vehicle.SetTransmission(new Transmission(request.Transmission));
        vehicle.SetEngineVolume(new EngineVolume(request.EngineVolume));
        vehicle.SetEnginePower(new EnginePower(request.EnginePower));
        vehicle.SetTractionType(new TractionType(request.TractionType));
        vehicle.SetFuelConsumption(new FuelConsumption(request.FuelConsumption));
        vehicle.SetSeatCount(new SeatCount(request.SeatCount));
        vehicle.SetKilometer(new Kilometer(request.Kilometer));
        vehicle.SetDailyPrice(new DailyPrice(request.DailyPrice));
        vehicle.SetWeeklyDiscountRate(new WeeklyDiscountRate(request.WeeklyDiscountRate));
        vehicle.SetMonthlyDiscountRate(new MonthlyDiscountRate(request.MonthlyDiscountRate));
        vehicle.SetInsuranceType(new InsuranceType(request.InsuranceType));
        vehicle.SetLastMaintenanceDate(new LastMaintenanceDate(request.LastMaintenanceDate));
        vehicle.SetLastMaintenanceKm(new LastMaintenanceKm(request.LastMaintenanceKm));
        vehicle.SetNextMaintenanceKm(new NextMaintenanceKm(request.NextMaintenanceKm));
        vehicle.SetInspectionDate(new InspectionDate(request.InspectionDate));
        vehicle.SetInsuranceEndDate(new InsuranceEndDate(request.InsuranceEndDate));
        vehicle.SetCascoEndDate(new CascoEndDate(request.CascoEndDate));
        vehicle.SetTireStatus(new TireStatus(request.TireStatus));
        vehicle.SetGeneralStatus(new GeneralStatus(request.GeneralStatus));
        vehicle.SetStatus(request.IsActive);
        vehicle.SetFeatures(request.Features.Select(f => new Feature(f)));

        _vehicleRepository.Update(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Vehicle updated successfully.";
    }
}
