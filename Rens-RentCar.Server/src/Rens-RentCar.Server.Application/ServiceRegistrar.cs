using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Rens_RentCar.Server.Application.Behaviors;
using TS.MediatR;

namespace Rens_RentCar.Server.Application;

public static class ServiceRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        // MediatR 
        services.AddMediatR(cfr =>
        {
            cfr.RegisterServicesFromAssembly(typeof(ServiceRegistrar).Assembly);

            // Dynamic Validation Behavior
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));

            // Dynamic Permission Behavior
            cfr.AddOpenBehavior(typeof(PermissionBehavior<,>));
        });

        //Fluent Validation
        services.AddValidatorsFromAssembly(typeof(ServiceRegistrar).Assembly);

        return services;
    }
}
