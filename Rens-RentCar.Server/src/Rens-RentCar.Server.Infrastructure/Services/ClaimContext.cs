using Microsoft.AspNetCore.Http;
using Rens_RentCar.Server.Application.Services;
using System.Security.Claims;

namespace Rens_RentCar.Server.Infrastructure.Services;

internal sealed class ClaimContext(IHttpContextAccessor _httpContextAccessor) : IClaimContext
{
    public Guid GetUserId()
    {
        var http = _httpContextAccessor.HttpContext;

        if (http is null)
            throw new ArgumentNullException(nameof(http));

        var claims = http.User.Claims;
        string? userId = (claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value) ?? throw new ArgumentException("User info not found");
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

    public Guid GetBranchId()
    {
        var http = _httpContextAccessor.HttpContext;

        if (http is null)
            throw new ArgumentNullException(nameof(http));

        var claims = http.User.Claims;
        string? branchId = (claims.FirstOrDefault(u => u.Type == "branchId")?.Value) ?? throw new ArgumentException("Branch info not found");
        try
        {
            Guid id = Guid.Parse(branchId);

            return id;
        }
        catch (Exception)
        {

            throw new ArgumentException("An error occurred while attempting branch Id parse to Guid");
        }
    }
}
