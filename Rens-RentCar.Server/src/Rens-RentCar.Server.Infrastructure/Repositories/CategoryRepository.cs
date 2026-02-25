using Rens_RentCar.Domain.Categories;
using Rens_RentCar.Server.Infrastructure.Abstractions;
using Rens_RentCar.Server.Infrastructure.Context;

namespace Rens_RentCar.Server.Infrastructure.Repositories;

internal sealed class CategoryRepository : AuditableRepository<Category, ApplicationDbContext>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
