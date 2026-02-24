using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Branches;
using Rens_RentCar.Server.Application.Roles;
using Rens_RentCar.Server.Application.Users;
using TS.MediatR;

namespace Rens_RentCar.Server.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]
public class BaseODataController : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<BranchDto>("branches");
        builder.EntitySet<RoleDto>("roles");
        builder.EntitySet<UserDto>("users");

        return builder.GetEdmModel();
    }
    [HttpGet("branches")]
    public IQueryable<BranchDto> Branches(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new BranchGetAllQuery(), cancellationToken).Result;

    [HttpGet("roles")]
    public IQueryable<RoleDto> Roles(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new RoleGetAllQuery(), cancellationToken).Result;

    [HttpGet("users")]
    public IQueryable<UserDto> Users(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new UserGetAllQuery(), cancellationToken).Result;

}
