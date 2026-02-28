using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Rens_RentCar.Server.Application;
using Rens_RentCar.Server.Infrastructure;
using Rens_RentCar.Server.WebAPI;
using Rens_RentCar.Server.WebAPI.Controllers;
using Rens_RentCar.Server.WebAPI.Middlewares;
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
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("forgot-password-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(2);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("reset-password-fixed", opt =>
    {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddRateLimiter(cfr =>
{
    cfr.AddFixedWindowLimiter("check-forgot-password-code-fixed", opt =>
    {
        opt.PermitLimit = 2;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

// Odata Configuration
builder.Services.AddControllers()
    .AddOData(opt =>
    opt
        .Select()
        .Filter()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)
        .AddRouteComponents("odata", BaseODataController.GetEdmModel())
    );

// CORS policy
builder.Services.AddCors();

// Check Token Middleware
builder.Services.AddTransient<CheckTokenMiddleware>();

// background service
builder.Services.AddHostedService<CheckLoginBackgroundService>();


builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

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
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.UseMiddleware<CheckTokenMiddleware>();

app.UseRateLimiter();

app.MapControllers()
    .RequireAuthorization()
    .RequireRateLimiting("fixed");

app.MapGet("/", () => "Hello World").RequireAuthorization();

//await app.AddSeedUser();
app.MapEndPoints();

await app.CleanRemovedPermissionsFromRoleAsync();
app.Run();
