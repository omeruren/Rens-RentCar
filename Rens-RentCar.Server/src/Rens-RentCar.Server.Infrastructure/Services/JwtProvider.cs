using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.LoginTokens;
using Rens_RentCar.Domain.LoginTokens.ValueObjects;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Services;
using Rens_RentCar.Server.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Rens_RentCar.Server.Infrastructure.Services;

internal sealed class JwtProvider(
    IOptions<JwtOptions> _jwtOptions,
    ILoginTokenRepository _loginTokenRepository,
    IRoleRepository _roleRepository,
    IBranchRepository _branchRepository,
    IUnitOfWork _unitOfWork) : IJwtProvider
{
    public async Task<string> CreateJwtTokenAsync(User user, CancellationToken cancellationToken = default)
    {

        var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == user.RoleId, cancellationToken);
        var branch = await _branchRepository.FirstOrDefaultAsync(b => b.Id == user.BranchId, cancellationToken);


        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim("fullName",$"{user.FirstName.Value } {user.LastName.Value}"),
            new Claim("fullNameWithEmail",user.FullName.Value),
            new Claim("email",user.Email.Value ),
            new Claim("role", role?.Name.Value ?? string.Empty),
            new Claim("permissions", role is null ? "": JsonSerializer.Serialize(role.Permissions.Select(s=>s.Value).ToArray())),
            new Claim("branch",branch?.Name.Value ?? string.Empty)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.Now.AddDays(1);

        JwtSecurityToken securityToken = new(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(securityToken);


        Token newToken = new(token);
        ExpiresDate expiresDate = new(expires);
        LoginToken loginToken = new(newToken, user.Id, expiresDate);

        _loginTokenRepository.Add(loginToken);

        var loginTokens = await _loginTokenRepository.Where(p => p.UserId == user.Id && p.IsActive.Value == true).ToListAsync(cancellationToken);

        foreach (var item in loginTokens)
            item.SetIsActive(new(false));

        _loginTokenRepository.UpdateRange(loginTokens);
        await _unitOfWork.SaveChangesAsync(cancellationToken);



        return token;
    }
}
