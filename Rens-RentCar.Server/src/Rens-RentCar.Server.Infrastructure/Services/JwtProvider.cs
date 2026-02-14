using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Services;
using Rens_RentCar.Server.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rens_RentCar.Server.Infrastructure.Services;

internal sealed class JwtProvider(IOptions<JwtOptions> _jwtOptions) : IJwtProvider
{
    public string CreateJwtToken(User user)
    {

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim("fullName",user.FullName.Value),
            new Claim("email",user.Email.Value)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);


        JwtSecurityToken securityToken = new(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(securityToken);

        return token;
    }
}
