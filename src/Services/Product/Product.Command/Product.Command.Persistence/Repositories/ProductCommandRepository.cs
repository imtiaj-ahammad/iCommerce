using Product.Command.Application;

namespace Product.Command.Persistence;

public class ProductCommandRepository : GenericCommandRepository<Product.Command.Domain.Product>, IProductCommandRepository
{
    public ProductCommandRepository(MssqlDbContext context) : base(context)
    {

    }
}