using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Rens_RentCar.Server.Application;
using Rens_RentCar.Server.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Context Accessor
builder.Services.AddHttpContextAccessor();

// Service Registrations
builder.Services.AddInfrastructure(builder.Configuration).AddApplication();

// Rate Limiting
builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

// Odata Configuration
builder.Services.AddControllers()
    .AddOData(opt =>
    opt
        .Select()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)

    );

// CORS policy
builder.Services.AddCors();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

// CORS Configuration
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("fixed");

app.MapGet("/", () => "Hello World!");

//await app.AddSeedUser();

app.Run();
