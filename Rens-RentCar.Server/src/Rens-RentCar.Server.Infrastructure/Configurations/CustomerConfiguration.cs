using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rens_RentCar.Domain.Customers;

namespace Rens_RentCar.Server.Infrastructure.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.NationalId);
        builder.OwnsOne(c => c.FirstName);
        builder.OwnsOne(c => c.LastName);
        builder.OwnsOne(c => c.FullName);
        builder.OwnsOne(c => c.BirthDate);
        builder.OwnsOne(c => c.PhoneNumber);
        builder.OwnsOne(c => c.Email);
        builder.OwnsOne(c => c.DrivingLicenceIssuanceDate);
        builder.OwnsOne(c => c.FullAddress);
    }
}
