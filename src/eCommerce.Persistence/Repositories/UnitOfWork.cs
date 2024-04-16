using eCommerce.Application;

namespace eCommerce.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MssqlDbContext _context;
    public UnitOfWork(MssqlDbContext context)
    {
        this._context = context;
        ProductCategoryQueryRepository = new ProductCategoryQueryRepository(_context);
        ProductCategoryCommandRepository = new ProductCategoryCommandRepository(_context);
    }

    public IProductCategoryQueryRepository ProductCategoryQueryRepository { get; private set; }
    public IProductCategoryCommandRepository ProductCategoryCommandRepository { get; private set; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
}
