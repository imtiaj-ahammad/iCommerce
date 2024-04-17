namespace Product.Command.Persistence;

public class ProductCommandRepository : GenericCommandRepository<Product>, IProductCommandRepository
{
    public ProductCommandRepository(MssqlDbContext context) : base(context)
    {

    }
}