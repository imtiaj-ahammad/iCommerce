namespace eCommerce.Application;

public interface IUnitOfWork : IDisposable
{
    IProductCategoryQueryRepository ProductCategoryQueryRepository { get; }
    IProductCategoryCommandRepository ProductCategoryCommandRepository { get; }
	int Save();
}
