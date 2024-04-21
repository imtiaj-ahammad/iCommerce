namespace Product.Query.Application;

public interface IUnitOfWork : IDisposable
{
    IProductQueryRepository ProductQueryRepository { get; }
    Task<bool> SaveChangesAsync();
}