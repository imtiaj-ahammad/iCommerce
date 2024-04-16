using eCommerce.Application;
using eCommerce.Domain;

namespace eCommerce.Persistence;

public class ProductCategoryQueryRepository : GenericQueryRepository<ProductCategory>, IProductCategoryQueryRepository
{
    public ProductCategoryQueryRepository(MssqlDbContext context) : base(context)
    {
        
    }
}
