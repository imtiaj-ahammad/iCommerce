namespace Product.Command.Application;
public interface IUnitOfWork : IDisposable
{
    IProductCommandRepository ProductCommandRepository { get; }
	int Save();
}