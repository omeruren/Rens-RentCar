using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

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

        return builder.GetEdmModel();
    }
}
