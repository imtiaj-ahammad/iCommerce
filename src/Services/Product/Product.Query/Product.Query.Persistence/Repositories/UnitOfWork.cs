using Product.Query.Application;

namespace Product.Query.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MongoDbContext _context;
    public UnitOfWork(MongoDbContext context)
    {
        this._context = context;
        ProductQueryRepository = new ProductQueryRepository(_context);
    }

    public IProductQueryRepository ProductQueryRepository { get; private set; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync(true) > 0;
    }
}