using eCommerce.Application;
using eCommerce.Domain;

namespace eCommerce.Persistence;

public class ProductCategoryCommandRepository : GenericCommandRepository<ProductCategory>, IProductCategoryCommandRepository
{
    public ProductCategoryCommandRepository(MssqlDbContext context) : base(context)
    {

    }

}
