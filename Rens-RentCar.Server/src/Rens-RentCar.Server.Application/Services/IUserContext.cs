namespace Rens_RentCar.Server.Application.Services;

public interface IUserContext
{
    Guid GetUserId(); // Will be received from token

}
