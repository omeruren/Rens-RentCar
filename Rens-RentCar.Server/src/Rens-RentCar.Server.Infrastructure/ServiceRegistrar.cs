using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rens_RentCar.Server.Infrastructure.Context;
using Rens_RentCar.Server.Infrastructure.Options;
using Scrutor;

namespace Rens_RentCar.Server.Infrastructure;

public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Jwt Options 
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        // Jwt Authentication Setup configuration
        services.ConfigureOptions<JwtSetupOptions>();

        // Authentication
        services.AddAuthentication().AddJwtBearer();

        // Authorization
        services.AddAuthorization();

        // Context Accessor
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlConnection")!;
            opt.UseSqlServer(connectionString);
        });

        //UnitOfWork
        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        // Dynamic Service Registration
        services.Scan(action => action
        .FromAssemblies(typeof(ServiceRegistrar).Assembly)
        .AddClasses(publicOnly: false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );

        return services;
    }
}
