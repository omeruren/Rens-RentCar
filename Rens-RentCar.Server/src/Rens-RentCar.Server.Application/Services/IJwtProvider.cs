using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Server.Application.Services;

public interface IJwtProvider
{
    Task<string> CreateJwtTokenAsync(User user, CancellationToken cancellationToken = default);
}
