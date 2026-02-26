using GenericRepository;
using Rens_RentCar.Domain.Abstraction;
using Rens_RentCar.Domain.Branches;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Shared;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Domain.Users.ValueObjects;
using Rens_RentCar.Server.Application.Services;
using Rens_RentCar.Server.WebAPI.Modules;

namespace Rens_RentCar.Server.WebAPI;

public static class ExtensionMethods
{
    public static async Task AddSeedUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var srv = scoped.ServiceProvider;
        var userRepository = srv.GetRequiredService<IUserRepository>();
        var unitOfWork = srv.GetRequiredService<IUnitOfWork>();
        var roleRepository = srv.GetRequiredService<IRoleRepository>();
        var branchRepository = srv.GetRequiredService<IBranchRepository>();


        Branch? branch = await branchRepository.FirstOrDefaultAsync(i => i.Name.Value == "Base Branch");

        Role? role = await roleRepository.FirstOrDefaultAsync(i => i.Name.Value == "sys_admin");

        if (branch is null)
        {
            Name name = new("Base Branch");
            Address address = new("Gaziantep", "NIZIP", "Gaziantep Nizip");
            Contact contact = new("05553258595", "05258967684", "info@rentcar.com");

            branch = new(name, address, contact, true);

            branchRepository.Add(branch);
        }

        if (role is null)
        {
            Name name = new("sys_admin");
            role = new(name, true);

            roleRepository.Add(role);

        }

        if (!(await userRepository.AnyAsync(u => u.UserName.Value == "admin")))
        {
            FirstName firstName = new("Omer");
            LastName lastName = new("Uren");
            Email email = new("admin@admin.com");
            UserName userName = new("admin");
            Password password = new("Admin123!");
            IdentityId branchId = branch.Id;
            IdentityId roleId = role.Id;
            var user = new User(firstName, lastName, email, userName, password, branchId, roleId, true);

            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public static void MapEndPoints(this WebApplication app)
    {
        app.MapAutEndpoint();
        app.MapBranchEndpoint();
        app.MapRoleEndPoint();
        app.MapPermissionEndpoint();
        app.MapUserEndpoint();
        app.MapCategoryEndpoint();
        app.MapProtectionEndpoint();
    }

    public static async Task CleanRemovedPermissionsFromRoleAsync(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();

        var srv = scoped.ServiceProvider.GetRequiredService<PermissionCleanerService>();

        await srv.CleanRemovedPermissionFromRoleAsync();
    }
}
