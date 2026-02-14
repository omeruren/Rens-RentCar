using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Server.Application.Services;

public interface IJwtProvider
{
    string CreateJwtToken(User user);
}
