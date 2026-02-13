using Microsoft.AspNetCore.Http;
using Rens_RentCar.Server.Application.Services;
using System.Security.Claims;

namespace Rens_RentCar.Server.Infrastructure.Services;

internal sealed class UserContext(IHttpContextAccessor _httpContextAccessor) : IUserContext
{
    public Guid GetUserId()
    {
        var http = _httpContextAccessor.HttpContext;

        var claims = http.User.Claims;
        string? userId = claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            throw new ArgumentException("User info not found");

        try
        {
            Guid id = Guid.Parse(userId);

            return id;
        }
        catch (Exception)
        {

            throw new ArgumentException("An error occurred while attempting Id parse to Guid");
        }
    }
}
