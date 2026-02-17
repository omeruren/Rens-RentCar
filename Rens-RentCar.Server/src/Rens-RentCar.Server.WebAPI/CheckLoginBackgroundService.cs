using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Rens_RentCar.Domain.LoginTokens;

namespace Rens_RentCar.Server.WebAPI;

public sealed class CheckLoginBackgroundService(IServiceScopeFactory _serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scoped = _serviceScopeFactory.CreateScope();

        var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();  // create unitOfWork instance
        var loginTokenRepository = scoped.ServiceProvider.GetRequiredService<ILoginTokenRepository>(); // create  loginTokenRepository instance

        var now = DateTimeOffset.Now;

        var activeTokens = await loginTokenRepository.Where(p => p.IsActive.Value == true && p.ExpiresDate.Value < now).ToListAsync(stoppingToken); // get active and expired tokens


        foreach (var item in activeTokens)
            item.SetIsActive(new(false));

        if (activeTokens.Any())
        {

            loginTokenRepository.UpdateRange(activeTokens);

            await unitOfWork.SaveChangesAsync(stoppingToken);
        }

        await Task.Delay(TimeSpan.FromDays(1)); // sleep for 1 day

    }
}
