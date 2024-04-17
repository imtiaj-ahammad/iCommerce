using Product.Command.Domain;

namespace Product.Command.Application;

public interface IProductCommandRepository: IGenericRepository<Product.Command.Domain.Product>
{

}