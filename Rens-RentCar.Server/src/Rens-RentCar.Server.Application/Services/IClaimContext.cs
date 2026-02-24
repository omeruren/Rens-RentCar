namespace Rens_RentCar.Server.Application.Services;

public interface IClaimContext
{
    Guid GetUserId(); // Will be received from token

    Guid GetBranchId();

}
