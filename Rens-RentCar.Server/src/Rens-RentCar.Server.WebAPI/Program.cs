using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Rens_RentCar.Server.Application;
using Rens_RentCar.Server.Infrastructure;
using Rens_RentCar.Server.WebAPI;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Service Registrations
builder.Services.AddInfrastructure(builder.Configuration).AddApplication();

// Response Compression
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

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

builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.QueueLimit = 1;
        opt.Window = TimeSpan.FromMinutes(1);
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

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

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

app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();
app.UseExceptionHandler();

app.MapControllers()
    .RequireAuthorization()
    .RequireRateLimiting("fixed");

app.MapGet("/", () => "Hello World!").RequireAuthorization();

//await app.AddSeedUser();
app.MapEndPoints();

app.Run();
