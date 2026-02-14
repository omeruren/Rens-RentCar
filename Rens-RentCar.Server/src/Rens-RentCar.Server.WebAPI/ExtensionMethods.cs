using GenericRepository;
using Rens_RentCar.Domain.Users;
using Rens_RentCar.Domain.Users.ValueObjects;

namespace Rens_RentCar.Server.WebAPI;

public static class ExtensionMethods
{
    public static async Task AddSeedUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();

        var repository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();
        var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (!(await repository.AnyAsync(u => u.UserName.Value == "admin")))
        {
            FirstName firstName = new("Omer");
            LastName lastName = new("Uren");
            Email email = new("admin@admin.com");
            UserName userName = new("admin");
            Password password = new("Admin123!");

            var user = new User(firstName, lastName, email, userName, password);

            repository.Add(user);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
