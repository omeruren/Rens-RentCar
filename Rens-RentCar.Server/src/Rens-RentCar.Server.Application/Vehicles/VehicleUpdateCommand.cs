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

        string fileName = vehicle.ImageUrl.Value;
        if (request.File is not null && request.File.Length > 0)
            fileName = FileService.FileSaveToServer(request.File, "wwwroot/images/");

        Brand brand = new(request.Brand);
        Model model = new(request.Model);
        ModelYear modelYear = new(request.ModelYear);
        Color color = new(request.Color);
        Plate plate = new(request.Plate);
        IdentityId categoryId = new(request.CategoryId);
        IdentityId branchId = new(request.BranchId);
        VinNumber vinNumber = new(request.VinNumber);
        EngineNumber engineNumber = new(request.EngineNumber);
        Description description = new(request.Description);
        ImageUrl imageUrl = new(fileName);
        FuelType fuelType = new(request.FuelType);
        Transmission transmission = new(request.Transmission);
        EngineVolume engineVolume = new(request.EngineVolume);
        EnginePower enginePower = new(request.EnginePower);
        TractionType tractionType = new(request.TractionType);
        FuelConsumption fuelConsumption = new(request.FuelConsumption);
        SeatCount seatCount = new(request.SeatCount);
        Kilometer kilometer = new(request.Kilometer);
        DailyPrice dailyPrice = new(request.DailyPrice);
        WeeklyDiscountRate weeklyDiscountRate = new(request.WeeklyDiscountRate);
        MonthlyDiscountRate monthlyDiscountRate = new(request.MonthlyDiscountRate);
        InsuranceType insuranceType = new(request.InsuranceType);
        LastMaintenanceDate lastMaintenanceDate = new(request.LastMaintenanceDate);
        LastMaintenanceKm lastMaintenanceKm = new(request.LastMaintenanceKm);
        NextMaintenanceKm nextMaintenanceKm = new(request.NextMaintenanceKm);
        InspectionDate inspectionDate = new(request.InspectionDate);
        InsuranceEndDate insuranceEndDate = new(request.InsuranceEndDate);
        CascoEndDate cascoEndDate = new(request.CascoEndDate);
        TireStatus tireStatus = new(request.TireStatus);
        GeneralStatus generalStatus = new(request.GeneralStatus);
        IEnumerable<Feature> features = request.Features.Select(f => new Feature(f));

        vehicle.SetBrand(brand);
        vehicle.SetModel(model);
        vehicle.SetModelYear(modelYear);
        vehicle.SetColor(color);
        vehicle.SetPlate(plate);
        vehicle.SetCategoryId(categoryId);
        vehicle.SetBranchId(branchId);
        vehicle.SetVinNumber(vinNumber);
        vehicle.SetEngineNumber(engineNumber);
        vehicle.SetDescription(description);
        vehicle.SetImageUrl(imageUrl);
        vehicle.SetFuelType(fuelType);
        vehicle.SetTransmission(transmission);
        vehicle.SetEngineVolume(engineVolume);
        vehicle.SetEnginePower(enginePower);
        vehicle.SetTractionType(tractionType);
        vehicle.SetFuelConsumption(fuelConsumption);
        vehicle.SetSeatCount(seatCount);
        vehicle.SetKilometer(kilometer);
        vehicle.SetDailyPrice(dailyPrice);
        vehicle.SetWeeklyDiscountRate(weeklyDiscountRate);
        vehicle.SetMonthlyDiscountRate(monthlyDiscountRate);
        vehicle.SetInsuranceType(insuranceType);
        vehicle.SetLastMaintenanceDate(lastMaintenanceDate);
        vehicle.SetLastMaintenanceKm(lastMaintenanceKm);
        vehicle.SetNextMaintenanceKm(nextMaintenanceKm);
        vehicle.SetInspectionDate(inspectionDate);
        vehicle.SetInsuranceEndDate(insuranceEndDate);
        vehicle.SetCascoEndDate(cascoEndDate);
        vehicle.SetTireStatus(tireStatus);
        vehicle.SetGeneralStatus(generalStatus);
        vehicle.SetFeatures(features);
        vehicle.SetStatus(request.IsActive);

        _vehicleRepository.Update(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Vehicle updated successfully.";
    }
}
