using Rens_RentCar.Domain.LoginTokens;
using System.Security.Claims;

namespace Rens_RentCar.Server.WebAPI.Middlewares;

public sealed class CheckTokenMiddleware(ILoginTokenRepository _loginTokenRepository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {

        //if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        //{
        //    await next(httpContext);
        //    return;
        //}
        var token = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {

            await next(httpContext);
            return;
        }


        var userId = httpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;


        if (userId is null)
            throw new AppTokenEx();

        var isTokenAvaliable = await _loginTokenRepository.AnyAsync(p => p.UserId == userId && p.Token.Value == token && p.IsActive.Value == true);

        if (!isTokenAvaliable)
            throw new AppTokenEx();

        await next(httpContext);
    }
}

public sealed class AppTokenEx : Exception;
