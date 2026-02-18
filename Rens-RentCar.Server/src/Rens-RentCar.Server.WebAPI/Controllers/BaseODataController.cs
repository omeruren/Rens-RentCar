using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Rens_RentCar.Server.Application.Branches;
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

        return builder.GetEdmModel();
    }
    [HttpGet("branches")]
    public IQueryable<BranchDto> Branches(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new BranchGetAllQuery(), cancellationToken).Result;
}
