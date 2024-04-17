using Product.Command.Application;

namespace Product.Command.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MssqlDbContext _context;
    public UnitOfWork(MssqlDbContext context)
    {
        this._context = context;
        ProductCommandRepository = new ProductCommandRepository(_context);
    }

    public IProductCommandRepository ProductCommandRepository { get; private set; }

    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync(true) > 0;
    }
}