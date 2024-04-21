using Product.Query.Application;

namespace Product.Query.Persistence;

public class ProductQueryRepository : GenericQueryRepository<Product.Query.Domain.Product>, IProductQueryRepository
{
    public ProductQueryRepository(MongoDbContext context) : base(context)
    {

    }
}