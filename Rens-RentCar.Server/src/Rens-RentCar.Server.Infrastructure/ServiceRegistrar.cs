using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        // Mail Setting Option
        services.Configure<MailSettingOptions>(configuration.GetSection("MailSettingsConfiguration"));

        // Mail Service
        using var scoped = services.BuildServiceProvider().CreateScope();
        var mailSettings = scoped.ServiceProvider.GetRequiredService<IOptions<MailSettingOptions>>();

        if (string.IsNullOrEmpty(mailSettings.Value.UserId))
            services.AddFluentEmail(mailSettings.Value.Email).AddSmtpSender(mailSettings.Value.Smtp, mailSettings.Value.Port);
        else
            services.AddFluentEmail(mailSettings.Value.Email).AddSmtpSender(mailSettings.Value.Smtp, mailSettings.Value.Port, mailSettings.Value.UserId, mailSettings.Value.Password);

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
