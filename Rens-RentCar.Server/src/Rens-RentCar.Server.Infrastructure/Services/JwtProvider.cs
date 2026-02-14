using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Services;

namespace Rens_RentCar.Server.Infrastructure.Services;

internal sealed class JwtProvider : IJwtProvider
{
    public string CreateJwtToken(User user)
    {
        return "token";
    }
}
