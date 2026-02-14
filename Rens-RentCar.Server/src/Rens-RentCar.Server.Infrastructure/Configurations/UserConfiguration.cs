using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rens_RentCar.Domain.Users;

namespace Rens_RentCar.Server.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.OwnsOne(u => u.FirstName);
        builder.OwnsOne(u => u.LastName);
        builder.OwnsOne(u => u.FullName);
        builder.OwnsOne(u => u.Email);
        builder.OwnsOne(u => u.UserName);
        builder.OwnsOne(u => u.Password);
    }
}
