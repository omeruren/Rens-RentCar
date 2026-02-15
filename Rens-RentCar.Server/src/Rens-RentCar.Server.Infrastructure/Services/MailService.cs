using FluentEmail.Core;
using Rens_RentCar.Server.Application.Services;

namespace Rens_RentCar.Server.Infrastructure.Services;

internal sealed class MailService(IFluentEmail _fluentEmail) : IMailService
{
    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        var sendResponse = await _fluentEmail.To(to).Subject(subject).Body(body, isHtml: true).SendAsync(cancellationToken);

        if (!sendResponse.Successful)
            throw new ArgumentException(string.Join(", ", sendResponse.ErrorMessages));
    }
}
