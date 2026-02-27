using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Domain.Extras;
using Rens_RentCar.Domain.ProtectionPackages;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Server.Application.Branches;
using Rens_RentCar.Server.Application.Categories;
using Rens_RentCar.Server.Application.Extras;
using Rens_RentCar.Server.Application.ProtectionPackages;
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
        builder.EntitySet<CategoryDto>("categories");
        builder.EntitySet<ProtectionDto>("protection-packages");
        builder.EntitySet<ExtraDto>("extras");

        return builder.GetEdmModel();
    }
    [HttpGet("branches")]
    public IQueryable<BranchDto> Branches(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new BranchGetAllQuery(), cancellationToken).Result;

    [HttpGet("roles")]
    public IQueryable<RoleDto> Roles(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new RoleGetAllQuery(), cancellationToken).Result;

    [HttpGet("users")]
    public IQueryable<UserDto> Users(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new UserGetAllQuery(), cancellationToken).Result;

    [HttpGet("categories")]
    public IQueryable<CategoryDto> Categories(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new CategoryGetAllQuery(), cancellationToken).Result;

    [HttpGet("protection-packages")]
    public IQueryable<ProtectionDto> Protections(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new ProtectionGetAllQuery(), cancellationToken).Result;

    [HttpGet("extras")]
    public IQueryable<ExtraDto> Extras(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new ExtraGetAllQuery(), cancellationToken).Result;

}
