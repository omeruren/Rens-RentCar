using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rens_RentCar.Domain.LoginTokens;

namespace Rens_RentCar.Server.Infrastructure.Configurations;

internal sealed class LoginTokenConfiguration : IEntityTypeConfiguration<LoginToken>
{
    public void Configure(EntityTypeBuilder<LoginToken> builder)
    {
        builder.HasKey(l => l.Id);
        builder.OwnsOne(l => l.Token);
        builder.OwnsOne(l => l.UserId);
        builder.OwnsOne(l => l.ExpiresDate);
        builder.OwnsOne(l => l.IsActive);
    }
}
