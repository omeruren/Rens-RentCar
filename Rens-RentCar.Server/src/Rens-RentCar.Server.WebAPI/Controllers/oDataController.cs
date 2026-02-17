using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Rens_RentCar.Server.Application.Branches;
using TS.MediatR;

namespace Rens_RentCar.Server.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableQuery]
public class ODataController : ControllerBase
{
    public static IEdmModel GetModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<BranchDto>("branches");

        return builder.GetEdmModel();
    }
    [HttpGet("branches")]
    public IQueryable<BranchDto> Branches(ISender _sender, CancellationToken cancellationToken = default) => _sender.Send(new BranchGetAllQuery(), cancellationToken).Result;
}
